using System.Threading.Tasks;
using BotOperate.Models.DatabaseContext;
using BotOperate.Models.MicrosoftGraph;

namespace BotOperate.Services.Interfaces
{
    public interface IGraphApiService
    {
        Task<Team> CreateNewTeamForPosition(Ticket position, string token);
    }
}