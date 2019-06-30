using Newtonsoft.Json;

namespace BotOperate.Models.Commands
{
    public class ActionCommandBase
    {
        [JsonProperty("commandId")] 
        public string CommandId { get; set; }
    }
}