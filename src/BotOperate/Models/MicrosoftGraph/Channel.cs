using Newtonsoft.Json;

namespace BotOperate.Models.MicrosoftGraph
{
    public sealed class Channel : Entity
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}