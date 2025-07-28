using Hangfire.Logging;
using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Features.SendMessage;
using Hookpay.Modules.Notifications.Core.Messages.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.NotificationDispatch;

public record MessageEvent(int userId, Alert alert);
public class NotificationDispatcher : IConsumer<MessageEvent>
{
    private readonly ILogger<NotificationDispatcher> _logger;   
    private readonly IEnumerable<INotificationChannel> _notificationChannels;

    //private readonly Dictionary<string, object> keyValuePairs;

    public NotificationDispatcher(
        IEnumerable<INotificationChannel> notificationChannels,
        ILogger<NotificationDispatcher> logger)
    {
        _notificationChannels = notificationChannels;
        _logger = logger;
        //keyValuePairs = new Dictionary<string, object>();
        //keyValuePairs.Add(nameof(PushType.Email), new EmailChannel());
        //keyValuePairs.Add(nameof(PushType.InApp), new SignalRChannel());
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

        await channel.SendAsync(data);

        await SaveStateOutBoxMessageAsync();
                       
    }

    private INotificationChannel GetChannel(PushType pushType)
    {
        var channel = _notificationChannels.FirstOrDefault(x => x.PushType == pushType);
        //var channel2 = keyValuePairs.TryGetValue(nameof(pushType), out var data);
        if (channel == null)
            return null;
        return channel;
    }

    public Task SaveStateOutBoxMessageAsync()
    {
        Console.WriteLine("save in outbox");
        return Task.CompletedTask;
    }

}
