using Hangfire.Logging;
using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.NotificationDispatch;

public record MessageEvent(int target, Alert alert);
public class NotificationDispatcher : IConsumer<MessageEvent>
{
    private readonly ILogger<NotificationDispatcher> _logger;   
    private readonly IEnumerable<INotificationChannel> _notificationChannels;

    public NotificationDispatcher(
        IEnumerable<INotificationChannel> notificationChannels,
        ILogger<NotificationDispatcher> logger)
    {
        _notificationChannels = notificationChannels;
        _logger = logger;   
    }
    public async Task Consume(ConsumeContext<MessageEvent> context)
    {
        var data = context.Message.alert;

        _logger.LogInformation($"Message with id={Random.Shared.Next(1,50)} is processing");
        

        var channel = GetChannel(data.PushType);
        if (channel == null)
        {
            throw new InvalidOperationException("Not support method push type");

        }

        await channel.SendAsync(context.Message.target, data);

        await SaveStateOutBoxMessageAsync();
                       
    }

    private INotificationChannel GetChannel(PushType pushType)
    {
        var channel = _notificationChannels.FirstOrDefault(x => x.PushType == pushType);       
        if (channel is null)
            return null!;
        return channel;
    }

    public Task SaveStateOutBoxMessageAsync()
    {
        Console.WriteLine("save in outbox");
        return Task.CompletedTask;
    }

}
