using Newtonsoft.Json;

namespace BotOperate.Models.MicrosoftGraph
{
    public sealed class TeamPerRoleSettings
    {
        [JsonProperty("allowCreateUpdateChannels")]
        public bool AllowCreateUpdateChannels { get; set; }
        
        [JsonProperty("allowDeleteChannels")]
        public bool AllowDeleteChannels { get; set; }
    }
}