using Newtonsoft.Json;

namespace BotOperate.Models.TaskModule
{
    public class TaskModuleContinueResponse : TaskModuleResponseBase
    {
        [JsonProperty(PropertyName = "value")]
        public TaskModuleTaskInfo Value { get; set; }
    }
}