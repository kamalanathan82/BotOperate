using System.Threading.Tasks;
using Refit;
using BotOperate.Models.MicrosoftGraph;

namespace BotOperate.Services.Refit
{
    [Headers("Authorization: Bearer")]
    public interface IGraphRestApiService
    {
        [Get("/v1.0/me?$select=id,mail,userPrincipalName")]
        Task<User> Me();

        [Get("/v1.0/users/{upn}?$select=id,mail,userPrincipalName")]
        Task<User> GetUserByUpn(string upn);

        [Post("/v1.0/groups")]
        Task<Group> CreateGroup([Body(BodySerializationMethod.Json)] Group group);
        
        [Put("/v1.0/groups/{groupId}/team")]
        Task<Team> CreateTeamForGroup(string groupId, [Body(BodySerializationMethod.Json)] Team team);

        [Post("/v1.0/teams/{teamId}/channels")]
        Task<Channel> CreateChannelForTeam(string teamId, [Body(BodySerializationMethod.Json)] Channel channel);

        [Post("/v1.0/teams/{teamId}/installedApps")]
        Task AddAppToTeam(string teamId, [Body] TeamsApp teamsApp);

        [Get("/v1.0/appCatalogs/teamsApps?$filter=distributionMethod eq 'organization' and externalId eq '{teamsAppExternalId}'")]
        Task<EntityCollection<TeamsApp>> ListTeamsApps(string teamsAppExternalId);

        [Post("/v1.0/teams/{teamId}/channels/{channelId}/tabs")]
        Task AddTab(string teamId, string channelId, [Body] TeamsTab teamsTab);
    }
}