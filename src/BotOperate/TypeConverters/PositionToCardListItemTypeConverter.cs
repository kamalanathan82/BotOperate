using AutoMapper;
using Microsoft.Bot.Connector;
using BotOperate.Constants;
using BotOperate.Models.Bot;
using BotOperate.Models.DatabaseContext;

namespace BotOperate.TypeConverters
{
    public class PositionToCardListItemTypeConverter : ITypeConverter<Ticket, CardListItem>
    {
        public CardListItem Convert(Ticket position, CardListItem cardListItem, ResolutionContext context)
        {
            if (position is null)
            {
                return null;
            }

            if (cardListItem is null)
            {
                cardListItem = new CardListItem();
            }

            var botCommand = BotCommands.PositionsDetailsDialogCommand;
            if (context.Items.TryGetValue("botCommand", out var botCommandValue))
            {
                botCommand = botCommandValue.ToString();
            }

            cardListItem.Icon = null;// position.HiringManager.ProfilePicture;
            cardListItem.Type = CardListItemTypes.ResultItem;
            cardListItem.Title = $"<b>{position.Ticketid} - {position.Status}</b>";
            cardListItem.Subtitle = $"Issue: {position.Description} | Assigned To: {position.AssignTo.Name} | Days open: {position.DaysOpen}";
            cardListItem.Tap = new CardAction(ActionTypes.ImBack, value: $"{botCommand} {position.Ticketid}");

            return cardListItem;
        }
    }
}