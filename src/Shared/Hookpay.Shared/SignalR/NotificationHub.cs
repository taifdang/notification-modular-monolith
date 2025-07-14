using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.SignalR;

public class NotificationHub:Hub, INotificationHubService
{
    private readonly ILogger<NotificationHub> _logger;
    private readonly IHubContext<NotificationHub> _hub;
    public NotificationHub(
       ILogger<NotificationHub> logger,
       IHubContext<NotificationHub> hub
        )
    {
        _logger = logger;
        _hub = hub;
    }
    public async Task SendAllAsync(string message)
    {
        try
        {
            await _hub.Clients.All.SendAsync(message);
            _logger.LogError("[message_hub.send]::send success");
        }
        catch
        {
            _logger.LogError("[message_hub.error]::occur fail when send message");
        }
    }
    public override Task OnConnectedAsync()
    {
        _logger.LogCritical($"[signalR]::id_{Context.ConnectionId} connected at {DateTime.Now}");
        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogWarning($"id_{Context.ConnectionId} disconnected at {DateTime.Now}");
        return base.OnDisconnectedAsync(exception);
    }
}
