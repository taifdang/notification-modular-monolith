
using BuildingBlocks.Signalr.Repository;
using BuildingBlocks.Web;
using Grpc.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Signalr;

//ref:https://learn.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-9.0
//ref:https://learn.microsoft.com/en-us/aspnet/core/signalr/hubcontext?view=aspnetcore-9.0
//ref:https://learn.microsoft.com/en-us/answers/questions/859091/how-to-get-the-userid-of-a-user-in-signalr-hub
//ref:https://learn.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/mapping-users-to-connections 
//ref:https://learn.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/hubs-api-guide-server
[Authorize(Policy = nameof(SignalrSchema))]
public class SignalrHub(
    ILogger<SignalrHub> logger,
    IHubContext<SignalrHub> hubContext,
    IHttpContextAccessor httpContextAccessor,
    IServiceScopeFactory serviceScopeFactory)
    : Hub, ISignalrHub
{
    public async Task BoardCastAsync(string message, CancellationToken cancellationToken = default)
    {
        try
        {
            await hubContext.Clients.All.SendAsync(message);

            logger.LogInformation("Message boardcast is sent");
        }
        catch(System.Exception ex)
        {
            throw new System.Exception("Occur fail when send message boardcast, please try again", ex);
        }
    }

    //server -> client = context = null
    //client -> server = context = valid (connectionId/userName/userId)
    public async Task ProcessAsync(string userId, string message, CancellationToken cancellationToken = default)
    {
        try
        {      
            await hubContext.Clients.User(userId).SendAsync(          
                message,
                cancellationToken);

            logger.LogInformation("Message with userId: {userId} sent", userId);
        }
        catch (System.Exception ex)
        {
            throw new System.Exception("Occur fail when send message personal, please try again", ex);
        }
    }

    //client -> server = context used (valid)
    public override async Task OnConnectedAsync()
    {
        if (Context?.User?.Identity?.IsAuthenticated == true)
        {
            var userId = Context.UserIdentifier ?? string.Empty;

            //httpContextAccessor.HttpContext?.GetTokenExpiryTime();
            var tokenExpiry = Context.User?.FindFirst("exp")?.Value;

            var scopeService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IConnectionProcessor>();
            //add group by userId: send transactionId
            await scopeService.AddConnectionAsync(Guid.Parse(userId), Context.ConnectionId, long.Parse(tokenExpiry));
         
            logger.LogWarning(
                "User with id: {UserId} connected at {DateTime}",
                userId,
                DateTime.Now.ToString());

            await base.OnConnectedAsync();
        }
        await Task.CompletedTask;
    }

    public override async Task OnDisconnectedAsync(System.Exception? exception)
    {
        var userId = Context.UserIdentifier;
        var connectionId = Context.ConnectionId;

        if (!string.IsNullOrEmpty(userId))
        {
            var scopeService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IConnectionProcessor>();

            await scopeService.RemoveConnectionAsync(Guid.Parse(userId), connectionId);
         
            //await Groups.RemoveFromGroupAsync(connectionId, transactionId); 

            logger.LogInformation($"User {userId} disconnected connectionId {connectionId} at {DateTime.Now.ToString()}");
        }
        else
        {
            logger.LogInformation($"An unauthenticated user disconnected connectionId {connectionId} at {DateTime.Now.ToString()}");
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.SendAsync("GroupJoined", groupName);
    }

    public async Task AddToGroupAsync(string connectionId, string groupName)
    {
        await Groups.AddToGroupAsync(connectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.SendAsync("GroupLeft", groupName);
    }
    public Task SendInGroupAsync(string groupName, string message)
    {
        return Clients.Group(groupName).SendAsync("GroupMessage", message);
    }
}
