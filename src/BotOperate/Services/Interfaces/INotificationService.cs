using System.Threading;
using System.Threading.Tasks;
using BotOperate.Models.DatabaseContext;

namespace BotOperate.Services.Interfaces
{
    public interface INotificationService
    {
        Task AddSubscriber(string webhookUrl, CancellationToken cancellationToken = default);

        Task RemoveSubscriber(string webhookUrl, CancellationToken cancellationToken = default);

        Task NotifyAboutStageChange(Candidate candidate, CancellationToken cancellationToken = default);
        
        Task NotifyRecruiterAboutNewOpenPosition(Ticket position, CancellationToken cancellationToken = default);
    }
}
