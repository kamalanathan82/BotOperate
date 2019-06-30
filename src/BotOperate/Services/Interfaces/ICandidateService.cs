using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using BotOperate.Models.Commands;
using BotOperate.Models.DatabaseContext;

namespace BotOperate.Services.Interfaces
{
    public interface ICandidateService
    {
        Task AddComment(LeaveCommentCommand leaveCommentCommand, string authorName, CancellationToken cancellationToken = default);
        Task UpdateCandidateStage(int candidateId, InterviewStageType newStage, CancellationToken cancellationToken = default);
        Task<ReadOnlyCollection<Candidate>> Search(string searchText, int maxResults, CancellationToken cancellationToken = default);
        Task<Candidate> GetById(int id);
    }
}