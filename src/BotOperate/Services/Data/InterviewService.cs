using System.Threading;
using System.Threading.Tasks;
using BotOperate.Models.Commands;
using BotOperate.Models.DatabaseContext;
using BotOperate.Services.Interfaces;

namespace BotOperate.Services.Data
{
    public sealed class InterviewService : IInterviewService
    {
        private readonly DatabaseContext _databaseContext;

        public InterviewService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public async Task ScheduleInterview(ScheduleInterviewCommand scheduleInterviewCommand, CancellationToken cancellationToken = default)
        {
            var candidate = await _databaseContext.Candidates.FindAsync(scheduleInterviewCommand.CandidateId);
            if (candidate != null)
            {
                candidate.Stage = InterviewStageType.Interviewing;
                
                await _databaseContext.Interviews.AddAsync(new Interview
                {
                    CandidateId = candidate.CandidateId,
                    InterviewDate = scheduleInterviewCommand.InterviewDate,
                    RecruiterId = scheduleInterviewCommand.InterviewerId,
                    FeedbackText = "N/A"
                }, cancellationToken);
                
                await _databaseContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}