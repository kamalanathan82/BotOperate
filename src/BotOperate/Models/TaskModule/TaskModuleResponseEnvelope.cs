using Newtonsoft.Json;

namespace BotOperate.Models.TaskModule
{
    public class TaskModuleResponseEnvelope
    {
        [JsonProperty(PropertyName = "task")]
        public TaskModuleResponseBase Task { get; set; }

    }
}