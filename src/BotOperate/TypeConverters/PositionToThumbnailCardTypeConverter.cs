using AutoMapper;
using Microsoft.Bot.Connector;
using BotOperate.Models.DatabaseContext;
using BotOperate.Models.Extensions;

namespace BotOperate.TypeConverters
{
    public class PositionToThumbnailCardTypeConverter : ITypeConverter<Ticket, ThumbnailCard>
    {
        public ThumbnailCard Convert(Ticket ticket, ThumbnailCard card, ResolutionContext _)
        {
            if (ticket is null)
            {
                return null;
            }

            if (card is null)
            {
                card = new ThumbnailCard();
            }

            card.Title = $"{ticket.Ticketid} / {ticket.Status}";
            card.Text = $"Desc: {ticket.Description}";
            
            return card;
        }
    }
}