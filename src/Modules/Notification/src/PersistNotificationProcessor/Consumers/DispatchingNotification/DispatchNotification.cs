using BuildingBlocks.Contracts;
using MassTransit;
using Notification.Data;

namespace Notification.PersistNotificationProcessor.Consumers.DispatchingNotification;

public class DispatchNotification : IConsumer<NotificationRendered>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly NotificationDbContext _notificationDbContext;

    public DispatchNotification(IPublishEndpoint publishEndpoint, NotificationDbContext notificationDbContext)
    {
        _publishEndpoint = publishEndpoint;
        _notificationDbContext = notificationDbContext;
    }

    public async Task Consume(ConsumeContext<NotificationRendered> context)
    {
        var @event = context.Message.NotificationMessage;

        if(@event is null)
            return;

        //save log to db if required
        var notificationLog = NotificationLogs.Model.NotificationLog.Create(
            Guid.NewGuid(), context.Message.Id, @event.Channel);

        await _notificationDbContext.NotificationLogs.AddAsync(notificationLog);

        await _notificationDbContext.SaveChangesAsync();

        //only for rabbitmq, with inmemory is skip -> filter in consumer
        await _publishEndpoint.Publish(@event, ctx =>
        {
            ctx.SetRoutingKey(@event.Channel.ToString());
        });

    }
}
