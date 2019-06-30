using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using BotOperate.Constants;
using BotOperate.Models.Bot;
using BotOperate.Services.Interfaces;

namespace BotOperate.Dialogs
{
	[Serializable]
    public class OpenTicketsDialog : IDialog<object>
	{
		private readonly ITicketService _positionService;
		private readonly IMapper _mapper;

		public OpenTicketsDialog(ITicketService positionService, 
			IMapper mapper)
		{
			_positionService = positionService;
			_mapper = mapper;
		}

		public async Task StartAsync(IDialogContext context)
		{
			var reply = context.MakeMessage();
			
			var OpenTickets = await _positionService.GetOpenTickets(context.Activity.From.Name, context.CancellationToken);
			if (OpenTickets.Any())
			{
				var title = $"You have {OpenTickets.Count} ticket open currently:";
			
				var cardListItems = _mapper.Map<List<CardListItem>>(OpenTickets);
				
				var attachment = new Attachment
				{
					ContentType = ListCard.ContentType,
					Content = new ListCard
					{
						Title = title,
						Items = cardListItems,
						Buttons = new List<CardAction>
						{
							new CardAction(ActionTypes.ImBack, "Submit new ticket", value:$"{BotCommands.NewTicketDialog}")
						}
					}
				};
				
				reply.Attachments = new List<Attachment>
				{
					attachment
				};
			}
			else
			{
				reply.Text = "You have no open positions";
			}

			await context.PostAsync(reply);
			context.Done(string.Empty);
		}
	}
}