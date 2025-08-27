
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BuildingBlocks.Signalr;

//[Authorize(Policy = nameof(SignalrSchema))]
public class SignalrHub(
    ILogger<SignalrHub> logger,
    IHubContext<SignalrHub> hubContext)
    : Hub, ISignalrHub
{
    public async Task BoardCastAsync(string message, CancellationToken cancellationToken = default)
    {
        try
        {
            var username = Context.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            await hubContext.Clients.All.SendAsync(
                "Username:{UserName} have new message: {Message}",
                username,
                message);

            logger.LogInformation("Message with username: {Username} sent", username);
        }
        catch(System.Exception ex)
        {
            throw new System.Exception("Occur fail when send message boardcast, please try again", ex);
        }
    }

    //???
    public async Task ProcessAsync(string target, string message, CancellationToken cancellationToken = default)
    {
        //target = connectionId/userName/userId
        try
        {
            var username = Context.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            await hubContext.Clients.User(target).SendAsync(
                "Username:{UserName} have new message: {Message}",
                username,
                message);

            logger.LogInformation("Message with username: {Username} sent", username);
        }
        catch (System.Exception ex)
        {
            throw new System.Exception("Occur fail when send message personal, please try again", ex);
        }
    }

    public override Task OnConnectedAsync()
    {
        if (Context?.User?.Identity?.IsAuthenticated == true)
        {
            var userId = Context.UserIdentifier ?? string.Empty;

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
}
