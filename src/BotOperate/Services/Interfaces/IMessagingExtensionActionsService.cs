using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace BotOperate.Services.Interfaces
{
    public interface IMessagingExtensionActionsService
    {
        HttpResponseMessage HandleFetchTaskAction(HttpRequestMessage request, string command);
        Task<HttpResponseMessage> HandleSubmitAction(HttpRequestMessage request, Activity activity, CancellationToken cancellationToken = default);
        Task<HttpResponseMessage> HandleButtonClickEvent(HttpRequestMessage request, string commandId, Activity activity, CancellationToken cancellationToken = default);
    }
}