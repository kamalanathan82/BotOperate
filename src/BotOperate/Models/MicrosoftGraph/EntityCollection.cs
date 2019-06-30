using System.Collections.Generic;
using Newtonsoft.Json;

namespace BotOperate.Models.MicrosoftGraph
{
    public class EntityCollection<T>
    {
        [JsonProperty("value")]
        public List<T> Items { get; set; }
    }
}