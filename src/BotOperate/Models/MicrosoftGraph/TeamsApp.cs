﻿using Newtonsoft.Json;

namespace BotOperate.Models.MicrosoftGraph
{
    public sealed class TeamsApp : Entity
    {
        [JsonProperty("teamsApp@odata.bind")] 
        public string TeamsAppId { get; set; }

        [JsonProperty("externalId")]
        public string ExternalId { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}