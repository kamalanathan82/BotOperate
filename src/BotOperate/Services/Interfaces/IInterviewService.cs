using System.Threading;
using System.Threading.Tasks;
using BotOperate.Models.Commands;

namespace BotOperate.Services.Interfaces
{
    public interface IInterviewService
    {
        Task ScheduleInterview(ScheduleInterviewCommand scheduleInterviewCommand, CancellationToken cancellationToken = default);
    }
}