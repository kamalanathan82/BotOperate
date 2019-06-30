using Newtonsoft.Json;

namespace BotOperate.Models.TaskModule
{
    public class TaskModuleMessageResponse : TaskModuleResponseBase
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

    }
}