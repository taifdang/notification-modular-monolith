using Ardalis.GuardClauses;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Data;
using Notification.PersistNotificationProcessor.Contracts;
using System.Text.Json;

namespace Notification.PersistNotificationProcessor.Dispatching.DispatchingNotification;

public class DispatchNotificationHanlder : IConsumer<NotificationRendered>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly NotificationDbContext _notificationDbContext;
    private readonly ILogger<DispatchNotificationHanlder> _logger;

    public DispatchNotificationHanlder(IPublishEndpoint publishEndpoint, NotificationDbContext notificationDbContext,
        ILogger<DispatchNotificationHanlder> logger)
    {
        _publishEndpoint = publishEndpoint;
        _notificationDbContext = notificationDbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<NotificationRendered> context)
    {
        Guard.Against.Null(context.Message, nameof(NotificationRendered));

        _logger.LogInformation($"consumer for {nameof(NotificationRendered)} is started");

        var @event = context.Message.NotificationMessage;

        // create log record
        var notificationLog = NotificationLogs.Model.NotificationLog.Create(
            context.Message.NotificationMessage.RequestId, context.Message.Id, @event.Channel);

        await _notificationDbContext.NotificationLogs.AddAsync(notificationLog);

        await _notificationDbContext.SaveChangesAsync();

        var metadata = JsonSerializer.Deserialize<Dictionary<string, object>>(context.Message.dataSchema);

        await _publishEndpoint.Publish(@event, ctx =>
        {
            // transport: inmemory = skip, filter at consumer
            ctx.SetRoutingKey(@event.Channel.ToString());

            //foreach (var header in metadata)
            //{
            //    ctx.Headers.Set(header.Key, header.Value);
            //}
        });
    }
}
