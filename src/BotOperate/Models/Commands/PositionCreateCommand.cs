using System;
using Newtonsoft.Json;

namespace BotOperate.Models.Commands
{
    public sealed class TicketCreateCommand : ActionCommandBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ticketid")] 
        public string Ticketid { get; set; }

        [JsonProperty("description")] 
        public string Description { get; set; }
                
        //[JsonProperty("jobTitle")] 
        //public string JobTitle { get; set; }

        //[JsonProperty("jobLevel")] 
        //public int JobLevel { get; set; }

        //[JsonProperty("jobPostingDate")] 
        //public DateTimeOffset JobPostingDate { get; set; }

        //[JsonProperty("jobLocation")] 
        //public int JobLocation { get; set; }

        //[JsonProperty("jobDescription")] 
        //public string JobDescription { get; set; }

        //[JsonProperty("jobHiringManager")] 
        //public string JobHiringManager { get; set; }
    }
}