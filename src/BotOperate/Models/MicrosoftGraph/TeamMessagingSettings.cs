using Newtonsoft.Json;

namespace BotOperate.Models.MicrosoftGraph
{
    public sealed class TeamMessagingSettings
    {
        [JsonProperty("allowUserEditMessages")]
        public bool AllowUserEditMessages { get; set; }

        [JsonProperty("allowUserDeleteMessages")]
        public bool AllowUserDeleteMessages { get; set; }
    }
}