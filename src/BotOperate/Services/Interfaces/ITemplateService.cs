using System;
using AdaptiveCards;
using BotOperate.Models.DatabaseContext;

namespace BotOperate.Services.Interfaces
{
    public interface ITemplateService
    {
        AdaptiveCard GetAdaptiveCardForNewJobPosting(string description = null);
        AdaptiveCard GetAdaptiveCardForInterviewRequest(Candidate candidate, DateTime interviewDate);
    }
}
