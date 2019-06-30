using System.Collections.Generic;
using AdaptiveCards;
using AutoMapper;
using BotOperate.Models.DatabaseContext;
using BotOperate.Models.Extensions;

namespace BotOperate.TypeConverters
{
    public class PositionToAdaptiveCardTypeConverter : ITypeConverter<Ticket, AdaptiveCard>
    {
        public AdaptiveCard Convert(Ticket position, AdaptiveCard card, ResolutionContext context)
        {
            if (position is null)
            {
                return null;
            }

            if (card is null)
            {
                card = new AdaptiveCard();
            }

            card.Version = "1.0";
            card.Body = new List<AdaptiveElement>
            {
                new AdaptiveTextBlock(position.Ticketid)
                {
                    Weight = AdaptiveTextWeight.Bolder,
                    Size = AdaptiveTextSize.Medium
                },

                new AdaptiveFactSet
                {
                    Facts = new List<AdaptiveFact>
                    {
	                    new AdaptiveFact("Ticket ID:", position.Ticketid),
	                    new AdaptiveFact("Status:", position.Status),
	                    new AdaptiveFact("Days open:", position.DaysOpen.ToString()),
                        new AdaptiveFact("Assigned To:", position.AssignTo.Name)
                    }
                },

	            new AdaptiveTextBlock($"Description: {position.Description}")
	            {
					Wrap = true
				}
			};

            return card;
        }
    }
}