using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using BotOperate.Models.DatabaseContext;

namespace BotOperate.Services.Interfaces
{
    public interface ILocationService
    {
        Task<ReadOnlyCollection<Location>> GetAllLocations(CancellationToken cancellationToken = default);
    }
}