using AdaptiveCards;
using AutoMapper;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;
using BotOperate.Extensions;
using BotOperate.Models.DatabaseContext;

namespace BotOperate.TypeConverters
{
    public class PositionToComposeExtensionAttachmentTypeConverter : ITypeConverter<Ticket, ComposeExtensionAttachment>
    {
        public ComposeExtensionAttachment Convert(Ticket position, ComposeExtensionAttachment _, ResolutionContext context)
        {
            var card = context.Mapper.Map<AdaptiveCard>(position);
            var preview = context.Mapper.Map<ThumbnailCard>(position);
            
            return card.ToAttachment().ToComposeExtensionAttachment(preview.ToAttachment());
        }
    }
}