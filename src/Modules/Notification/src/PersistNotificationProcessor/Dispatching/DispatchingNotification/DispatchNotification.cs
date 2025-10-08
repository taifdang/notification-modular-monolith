using Ardalis.GuardClauses;
using MassTransit;
using Notification.Data;
using Notification.PersistNotificationProcessor.Contracts;

namespace Notification.PersistNotificationProcessor.Dispatching.DispatchingNotification;

public class DispatchNotificationHanlder : IConsumer<NotificationRendered>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly NotificationDbContext _notificationDbContext;

    public DispatchNotificationHanlder(IPublishEndpoint publishEndpoint, NotificationDbContext notificationDbContext)
    {
        _publishEndpoint = publishEndpoint;
        _notificationDbContext = notificationDbContext;
    }

    public async Task Consume(ConsumeContext<NotificationRendered> context)
    {
        Guard.Against.Null(context.Message, nameof(NotificationRendered));

        var @event = context.Message.NotificationMessage;

        //save log to db if required
        var notificationLog = NotificationLogs.Model.NotificationLog.Create(
            Guid.NewGuid(), context.Message.Id, @event.Channel);

        await _notificationDbContext.NotificationLogs.AddAsync(notificationLog);

        await _notificationDbContext.SaveChangesAsync();

        //transport: inmemory = skip, filter at consumer
        await _publishEndpoint.Publish(@event, ctx =>
        {
            ctx.SetRoutingKey(@event.Channel.ToString());
        });
    }
}
