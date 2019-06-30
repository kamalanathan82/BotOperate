using System;
using System.Collections.Generic;
using System.Linq;
using AdaptiveCards;
using Microsoft.Bot.Connector;
using BotOperate.Constants;
using BotOperate.Extensions;
using BotOperate.Models.DatabaseContext;
using BotOperate.Services.Interfaces;

namespace BotOperate.Services
{
    public sealed class TemplateService : ITemplateService
    {
        private readonly ILocationService _locationService;
        private readonly IRecruiterService _recruiterService;

        public TemplateService(ILocationService locationService,
            IRecruiterService recruiterService)
        {
            _locationService = locationService;
            _recruiterService = recruiterService;
        }

        public AdaptiveCard GetAdaptiveCardForNewJobPosting(string description = null)
        {
            var locations = _locationService.GetAllLocations().GetAwaiter().GetResult();
            var hiringManagers = _recruiterService.GetAllHiringManagers().GetAwaiter().GetResult();

            var command = new
            {
                commandId = AppCommands.OpenNewTicket
            };
            
            var wrapAction = new CardAction
            {
                Title = "Create ticket",
                Value = command
            };

            var action = new AdaptiveSubmitAction
            {
                Data = command
            };

            action.RepresentAsBotBuilderAction(wrapAction);

            return new AdaptiveCard
            {
                Version = "1.0",
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock("Enter in ticket description")
                    {
                        IsSubtle = true,
                        Wrap = true,
                        Size = AdaptiveTextSize.Small
                    },
                    new AdaptiveTextBlock("Description"),
                    new AdaptiveTextInput
                    {
                        Id = "description",
                        IsMultiline = true,
                        Placeholder = "E.g There is an application error.",
                        Value = description
                    }
                },
                Actions = new List<AdaptiveAction>
                {
                    action
                }
            };
        }

        public AdaptiveCard GetAdaptiveCardForInterviewRequest(Candidate candidate, DateTime interviewDate)
        {
            var interviewers = _recruiterService.GetAllInterviewers().GetAwaiter().GetResult();
            
            var command = new
            {
                commandId = AppCommands.ScheduleInterview,
                candidateId = candidate.CandidateId
            };
            
            var wrapAction = new CardAction
            {
                Title = "Schedule",
                Value = command
            };

            var action = new AdaptiveSubmitAction
            {
                Data = command
            };

            action.RepresentAsBotBuilderAction(wrapAction);
            
            return new AdaptiveCard
            {
                Version = "1.0",
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = $"Set interview date for {candidate.Name}",
                        Weight = AdaptiveTextWeight.Bolder,
                        Size = AdaptiveTextSize.Large
                    },
                    new AdaptiveColumnSet
                    {
                        Columns = new List<AdaptiveColumn>
                        {
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Auto,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveImage
                                    {
                                        Url = new Uri(candidate.ProfilePicture),
                                        Size = AdaptiveImageSize.Medium,
                                        Style = AdaptiveImageStyle.Person
                                    }
                                }
                            },
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Stretch,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = $"TicketID: {candidate.Position.Ticketid}",
                                        Wrap = true
                                    },
                                    new AdaptiveTextBlock
                                    {
                                        Text = $"Status: {candidate.Position.Status}",
                                        Spacing = AdaptiveSpacing.None,
                                        Wrap = true,
                                        IsSubtle = true
                                    }
                                }
                            }
                        }
                    },
                    new AdaptiveChoiceSetInput
                    {
                        Id = "interviewerId",
                        Style = AdaptiveChoiceInputStyle.Compact,
                        Choices = interviewers.Select(x => new AdaptiveChoice
                        {
                            Value = x.RecruiterId.ToString(),
                            Title = x.Name
                        }).ToList(),
                        Value = Convert.ToString(interviewers[0].RecruiterId)
                    },
                    new AdaptiveDateInput
                    {
                        Id = "interviewDate", Placeholder = "Enter in a date for the interview", Value = interviewDate.ToShortDateString()
                    },
                    new AdaptiveChoiceSetInput
                    {
                        Id = "interviewType",
                        Style = AdaptiveChoiceInputStyle.Compact,
                        IsMultiSelect = false,
                        Choices = new List<AdaptiveChoice>
                        {
                            new AdaptiveChoice {Title = "Phone screen", Value = "phoneScreen"},
                            new AdaptiveChoice {Title = "Full loop", Value = "fullLoop"},
                            new AdaptiveChoice {Title = "Follow-up", Value = "followUp"}
                        },
                        Value = "phoneScreen"
                    },
                    new AdaptiveToggleInput {Id = "isRemote", Title = "Remote interview"}
                },
                Actions = new List<AdaptiveAction>
                {
                    action
                }
            };
        }
    }
}