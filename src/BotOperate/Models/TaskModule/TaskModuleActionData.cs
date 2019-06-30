using Newtonsoft.Json;

namespace BotOperate.Models.TaskModule
{
    public class TaskModuleActionData<T>
    {
        [JsonProperty("data")] 
        public T Data { get; set; }
    }
}