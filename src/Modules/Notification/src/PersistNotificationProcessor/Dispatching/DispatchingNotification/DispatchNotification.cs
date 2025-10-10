using Ardalis.GuardClauses;
using MassTransit;
using Microsoft.Extensions.Logging;
using Notification.Data;
using Notification.PersistNotificationProcessor.Contracts;

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

        //save log to db if required
        var notificationLog = NotificationLogs.Model.NotificationLog.Create(
            context.Message.NotificationMessage.MessageId, context.Message.Id, @event.Channel);

        await _notificationDbContext.NotificationLogs.AddAsync(notificationLog);

        await _notificationDbContext.SaveChangesAsync();

        //transport: inmemory = skip, filter at consumer
        await _publishEndpoint.Publish(@event, ctx =>
        {
            ctx.SetRoutingKey(@event.Channel.ToString());
        });
    }
}
