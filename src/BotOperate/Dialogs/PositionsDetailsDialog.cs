using System;
using System.Threading.Tasks;
using AdaptiveCards;
using AutoMapper;
using Microsoft.Bot.Builder.Dialogs;
using BotOperate.Constants;
using BotOperate.Extensions;
using BotOperate.Models.DatabaseContext;
using BotOperate.Services.Interfaces;

namespace BotOperate.Dialogs
{
    [Serializable]
    public class PositionsDetailsDialog : IDialog<object>
    {
        private readonly ITicketService _ticketService;
        private readonly IMapper _mapper;

        public PositionsDetailsDialog(ITicketService ticketService, 
            IMapper mapper)
        {
            _ticketService = ticketService;
            _mapper = mapper;
        }

        public async Task StartAsync(IDialogContext context)
        {
            var reply = context.MakeMessage();
            var text = context.Activity.GetTextWithoutCommand(BotCommands.PositionsDetailsDialogCommand);
            Ticket position = null;
            if (text.HasValue())
            {
                if (int.TryParse(text, out var positionId))
                {
                    position = await _ticketService.GetById(positionId);
                } 
            }
            else
            {

                reply.Text = "Please specify Position ID.";
            }

            if (position is null)
            {
                reply.Text = "I couldn't find this position.";
            }
            else
            {
                var card = _mapper.Map<AdaptiveCard>(position);
                reply.Attachments.Add(card.ToAttachment());
            }

            await context.PostAsync(reply);
            context.Done(string.Empty);
        }
    }
}