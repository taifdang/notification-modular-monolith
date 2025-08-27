using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
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
        var messages =
            await _notificationDbContext.Messages.Where(x => x.Id == context.Message.Id).ToListAsync();

        if (messages is null || messages.Count() == 0)
            return;

        //only for rabbitmq, with inmemory is skip -> filter in consumer
        foreach (var item in messages)
        {
            await _publishEndpoint.Publish(new NotificationMessage(item.Channel), ctx =>
            {              
                ctx.SetRoutingKey(item.Channel.ToString());
            });
        }

    }
}
