﻿using System.Collections.Generic;

namespace BotOperate.Models.DatabaseContext
{
    public sealed class Recruiter
    {
        public Recruiter()
        {
            Positions = new List<Ticket>();
        }
        
        public int RecruiterId { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string ProfilePicture { get; set; }

        public RecruiterRole Role { get; set; }

        public string DirectReportIds { get; set; }

        public List<Ticket> Positions { get; set; }

        public TeamsChannelData TeamsChannelData { get; set; }
    }
}
