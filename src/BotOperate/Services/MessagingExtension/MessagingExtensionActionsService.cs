using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using AutoMapper;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams.Models;
using Newtonsoft.Json;
using BotOperate.Constants;
using BotOperate.Extensions;
using BotOperate.Models.Commands;
using BotOperate.Models.TaskModule;
using BotOperate.Services.Interfaces;

namespace BotOperate.Services.MessagingExtension
{
    public class MessagingExtensionActionsService : IMessagingExtensionActionsService
    {
        private readonly ITemplateService _templateService;
        private readonly ICandidateService _candidateService;
        private readonly IInterviewService _interviewService;
        private readonly ITicketService _positionService;
        private readonly IMapper _mapper;

        public MessagingExtensionActionsService(ITemplateService templateService,
            ICandidateService candidateService,
            IInterviewService interviewService,
            ITicketService positionService,
            IMapper mapper)
        {
            _templateService = templateService;
            _candidateService = candidateService;
            _interviewService = interviewService;
            _positionService = positionService;
            _mapper = mapper;
        }

        public HttpResponseMessage HandleFetchTaskAction(HttpRequestMessage request, string commandId)
        {
            if (string.Equals(commandId, MessagingExtensionCommands.OpenNewPosition, StringComparison.OrdinalIgnoreCase))
            {
                var card = _templateService.GetAdaptiveCardForNewJobPosting();

                var response = new TaskModuleResponseEnvelope
                {
                    Task = new TaskModuleContinueResponse
                    {
                        Type = "continue",
                        Value = new TaskModuleTaskInfo
                        {
                            Card = card.ToAttachment(),
                            Title = "Create new job posting",
                            Width = "large",
                            Height = "large"
                        }
                    }
                };

                return request.CreateResponse(HttpStatusCode.OK, response);
            }

            return request.CreateResponse(HttpStatusCode.NotFound);
        }

        public async Task<HttpResponseMessage> HandleSubmitAction(HttpRequestMessage request, Activity activity, CancellationToken cancellationToken)
        {
            var submitActionData = JsonConvert.DeserializeObject<TaskModuleActionData<TicketCreateCommand>>(activity.Value?.ToString());
            if (submitActionData?.Data is null)
            {
                return request.CreateResponse(HttpStatusCode.NoContent);
            }

            switch (submitActionData.Data.CommandId)
            {
                // Open task module confirming job posting
                case AppCommands.OpenNewTicket:
                {
                    var response = await CreateConfirmJobPostingTaskModuleResponse(submitActionData.Data, cancellationToken);
                    return request.CreateResponse(HttpStatusCode.OK, response);
                }
                // Insert card confirming the action
                case AppCommands.ConfirmCreationOfNewPosition:
                {
                    var response = await CreateResponseToConfirmCreatePostingCommand(submitActionData.Data);
                    return request.CreateResponse(HttpStatusCode.OK, response);
                }
            }

            return request.CreateResponse(HttpStatusCode.OK);
        }

        public async Task<HttpResponseMessage> HandleButtonClickEvent(HttpRequestMessage request, string commandId, Activity activity, CancellationToken cancellationToken)
        {
            switch (commandId)
            {
                case AppCommands.LeaveInternalComment:
                    var leaveCommentRequest = JsonConvert.DeserializeObject<LeaveCommentCommand>(activity?.Value?.ToString());
                    await _candidateService.AddComment(leaveCommentRequest, activity?.From.Name, cancellationToken);
                    break;
                case AppCommands.ScheduleInterview:
                    var scheduleInterviewRequest = JsonConvert.DeserializeObject<ScheduleInterviewCommand>(activity?.Value?.ToString());
                    await _interviewService.ScheduleInterview(scheduleInterviewRequest, cancellationToken);
                    break;
            }
            
            return request.CreateResponse(HttpStatusCode.OK);
        }

        private async Task<TaskModuleResponseEnvelope> CreateConfirmJobPostingTaskModuleResponse(TicketCreateCommand positionCreateCommand, CancellationToken cancellationToken)
        {
            var position = await _positionService.AddNewTicket(positionCreateCommand, cancellationToken);

            positionCreateCommand.CommandId = AppCommands.ConfirmCreationOfNewPosition;
            positionCreateCommand.Ticketid = position.Ticketid;

            var card = _mapper.Map<AdaptiveCard>(position);
            card.Actions = new List<AdaptiveAction>
            {
                new AdaptiveSubmitAction
                {
                    Title = "Confirm posting",
                    Data = positionCreateCommand
                },
                new AdaptiveSubmitAction
                {
                    Title = "Cancel"
                }
            };

            return new TaskModuleResponseEnvelope
            {
                Task = new TaskModuleContinueResponse
                {
                    Type = "continue",
                    Value = new TaskModuleTaskInfo
                    {
                        Card = card.ToAttachment(),
                        Title = "Confirm new posting",
                        Width = "medium",
                        Height = "medium"
                    }
                }
            };
        }

        private async Task<ComposeExtensionResponse> CreateResponseToConfirmCreatePostingCommand(TicketCreateCommand data)
        {
            var position = await _positionService.GetById(data.Id);
            var extensionAttachment = _mapper.Map<ComposeExtensionAttachment>(position);

            return new ComposeExtensionResponse
            {
                ComposeExtension = new ComposeExtensionResult
                {
                    AttachmentLayout = AttachmentLayoutTypes.List,
                    Type = "result",
                    Attachments = new List<ComposeExtensionAttachment>
                    {
                        extensionAttachment
                    }
                }
            };
        }
    }
}