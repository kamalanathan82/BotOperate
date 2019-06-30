using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BotOperate.Models.Commands;
using BotOperate.Models.DatabaseContext;
using BotOperate.Services.Interfaces;

namespace BotOperate.Services.Data
{
    public sealed class TicketService : ITicketService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public TicketService(DatabaseContext databaseContext,
            INotificationService notificationService,
            IMapper mapper)
        {
            _databaseContext = databaseContext;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<ReadOnlyCollection<Ticket>> GetAllPositions(CancellationToken cancellationToken)
            => (await _databaseContext.Tickets.ToListAsync(cancellationToken)).AsReadOnly();

        public async Task<ReadOnlyCollection<Ticket>> Search(string searchText, int maxResults, CancellationToken cancellationToken)
        {
            Ticket[] positions;
            if (string.IsNullOrEmpty(searchText))
            {
                positions = await _databaseContext.Tickets
                    .OrderBy(x => x.DaysOpen)
                    .Take(maxResults)
                    .ToArrayAsync(cancellationToken);
            }
            else
            {
                searchText = searchText.ToLowerInvariant();
                
                positions = await _databaseContext.Tickets
                    .Where(x => x.Ticketid.ToLowerInvariant().Contains(searchText))
                    .OrderBy(x => x.DaysOpen)
                    .Take(maxResults)
                    .ToArrayAsync(cancellationToken);
            }

            return Array.AsReadOnly(positions);
        }

        public Task<Ticket> GetById(int positionId)
        {
            return _databaseContext.Tickets.FindAsync(positionId);
        }

        public async Task<Ticket> AddNewTicket(TicketCreateCommand positionCreateCommand, CancellationToken cancellationToken)
        {
            var ticketdetails = _mapper.Map<Ticket>(positionCreateCommand);
            Random rn = new Random(10000000);
            ticketdetails.Id = rn.Next().ToString();
            ticketdetails.Ticketid = "T" + rn.Next().ToString();
            ticketdetails.CreatedDate = DateTime.Now;
            ticketdetails.Status = "Open";
            ticketdetails.AssignTo = new Recruiter() { Name = "Nathan S" };
            _databaseContext.Tickets.Add(ticketdetails);
            //_databaseContext.SaveChanges();
           await _databaseContext.SaveChangesAsync(cancellationToken);

            //await _notificationService.NotifyRecruiterAboutNewOpenPosition(ticketdetails, cancellationToken);

            return ticketdetails;
        }

        public async Task<ReadOnlyCollection<Ticket>> GetOpenTickets(string recruiterNameOrAlias, CancellationToken cancellationToken)
        {
            var recruiter = await _databaseContext.Recruiters.FirstOrDefaultAsync(x => 
                string.Equals(x.Alias, recruiterNameOrAlias, StringComparison.OrdinalIgnoreCase) || 
                string.Equals(x.Name, recruiterNameOrAlias), cancellationToken);
            
            Ticket[] positions;
            if (recruiter is null)
            {
                positions = await _databaseContext.Tickets.ToArrayAsync(cancellationToken);
            }
            else
            {
                positions = recruiter.Positions.ToArray();
            }

            return Array.AsReadOnly(positions);
        }

        //public Task<Ticket> GetByExternalId(string externalId, CancellationToken cancellationToken)
        //{
        //    return _databaseContext.Tickets.FirstOrDefaultAsync(
        //        x => string.Equals(x.PositionExternalId, externalId, StringComparison.OrdinalIgnoreCase),
        //        cancellationToken);
        //}
    }
}