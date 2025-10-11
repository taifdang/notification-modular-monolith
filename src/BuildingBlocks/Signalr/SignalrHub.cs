
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
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
    IHttpContextAccessor httpContextAccessor)
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
    public override Task OnConnectedAsync()
    {
        if (Context?.User?.Identity?.IsAuthenticated == true)
        {
            var userId = Context.UserIdentifier ?? string.Empty;

            //add group by userId: send transactionId

            //Groups.AddToGroupAsync(Context.ConnectionId, userId);

            logger.LogWarning(
                "User with id: {UserId} connected at {DateTime}",
                userId,
                DateTime.Now.ToString());

            return base.OnConnectedAsync();
        }
        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(System.Exception? exception)
    {
        logger.LogInformation(
            "A anoymous user disconnected at {DateTime}",
            DateTime.Now.ToString());

        return base.OnDisconnectedAsync(exception);
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
}
