using System.Collections.Generic;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace BotOperate.Models.Bot
{
	public class ListCard
	{
		public const string ContentType = "application/vnd.microsoft.teams.card.list";

		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }

		[JsonProperty(PropertyName = "items")]
		public IList<CardListItem> Items { get; set; }

		[JsonProperty(PropertyName = "buttons")]
		public IList<CardAction> Buttons { get; set; }
	}
}
