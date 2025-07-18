using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.SignalR;

[Authorize]
public class NotificationHub:Hub, INotificationHubService
{
    private readonly ILogger<NotificationHub> _logger;
    private readonly IHubContext<NotificationHub> _hub;
    public NotificationHub(
       ILogger<NotificationHub> logger,
       IHubContext<NotificationHub> hub,
       IHttpContextAccessor context
        )
    {
        _logger = logger;
        _hub = hub;
    }
    public async Task SendAllAsync(string message)
    {
        try
        {
            var userName = Context.User?.FindFirst("name")?.Value ?? string.Empty;
            await _hub.Clients.All.SendAsync($"{userName}::{message}");
            _logger.LogError("[message_hub.send]::send success");
        }
        catch(Exception ex) 
        {
            _logger.LogError($"[message_hub.error]::occur fail when send message:::{ex.ToString()}");
        }
    }
    public async Task SendPersonalAsync(string userId,string message)
    {
        try
        {        
            await _hub.Clients.User(userId).SendAsync($"{userId}::{message}");
            _logger.LogError("[message_hub.send]::send success");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[message_hub.error]::occur fail when send message:::{ex.ToString()}");
        }
    }
    public override Task OnConnectedAsync()
    {
        if (Context.User?.Identity?.IsAuthenticated == true)
        {
            var userId = Context.UserIdentifier;
            _logger.LogCritical($"[signalR]::id_{Context.ConnectionId} connected at {DateTime.Now}");
            return base.OnConnectedAsync();
        }
        _logger.LogCritical($"token isValid");
        return Task.CompletedTask;
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogWarning($"id_{Context.ConnectionId} disconnected at {DateTime.Now}");
        return base.OnDisconnectedAsync(exception);
    }
}
