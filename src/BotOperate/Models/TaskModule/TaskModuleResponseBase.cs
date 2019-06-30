using Newtonsoft.Json;

namespace BotOperate.Models.TaskModule
{
    public class TaskModuleResponseBase
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

    }
}