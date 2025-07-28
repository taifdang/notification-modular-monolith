
using Hookpay.Modules.Notifications.Core.Messages.Models;
using Hookpay.Shared.SignalR;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.SendMessage;

public record SignalrChannelQueue (int target, Alert alert);

public class SignalrChannelQueueHandler : IConsumer<SignalrChannelQueue>
{
    private readonly INotificationHubService _hub;
    private readonly ILogger<SignalrChannelQueueHandler> _logger;
    public SignalrChannelQueueHandler(
        INotificationHubService notificationHubService,
        ILogger<SignalrChannelQueueHandler> logger)
    {
        _hub = notificationHubService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SignalrChannelQueue> context)
    {
        try
        {
            var data = context.Message.alert;
            //await Task.Delay(100);
            //await _hub.SendAllAsync(context.Message.body);
            await _hub.SendPersonalAsync(context.Message.target.ToString(), data.Body);
            _logger.LogInformation($"Message with {data.Title} = {data.Body} is processed");

        }
        catch
        {
            _logger.LogCritical($"occure error");

        }
    }
}

