using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using BotOperate.Models.Commands;
using BotOperate.Models.DatabaseContext;

namespace BotOperate.Services.Interfaces
{
    public interface ITicketService
    {
        Task<ReadOnlyCollection<Ticket>> GetAllPositions(CancellationToken cancellationToken);
        Task<ReadOnlyCollection<Ticket>> Search(string searchText, int maxResults, CancellationToken cancellationToken = default);
        Task<Ticket> GetById(int positionId);
        Task<Ticket> AddNewTicket(TicketCreateCommand positionCreateCommand, CancellationToken cancellationToken = default);
        Task<ReadOnlyCollection<Ticket>> GetOpenTickets(string recruiterNameOrAlias, CancellationToken cancellationToken = default);
        //Task<Ticket> GetByExternalId(string externalId, CancellationToken cancellationToken = default);
    }
}