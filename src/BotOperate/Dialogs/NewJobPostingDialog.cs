using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using BotOperate.Extensions;
using BotOperate.Services.Interfaces;

namespace BotOperate.Dialogs
{
    [Serializable]
    public class NewTicketDialog : IDialog<object>
    {
        private readonly ITemplateService _templateService;

        public NewTicketDialog(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        public async Task StartAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();

            var card = _templateService.GetAdaptiveCardForNewJobPosting();
            reply.Attachments.Add(card.ToAttachment());

            await context.PostAsync(reply);
            context.Done(string.Empty);
        }
    }
}