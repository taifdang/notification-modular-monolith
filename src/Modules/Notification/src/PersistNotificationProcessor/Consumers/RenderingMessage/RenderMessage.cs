using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notification.Configurations.Templates;
using Notification.Data;
using Notification.Notifications.Model;

namespace Notification.PersistNotificationProcessor.Consumers.RenderingMessage;
public class RenderMessage : IConsumer<NotificationReadyToRender>
{
    private readonly NotificationDbContext _notificationDbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    public RenderMessage(NotificationDbContext notificationDbContext, IPublishEndpoint publishEndpoint)
    {
        _notificationDbContext = notificationDbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<NotificationReadyToRender> context)
    {
        var notification =
            await _notificationDbContext.Notifications.FirstOrDefaultAsync(x => x.Id == context.Message.Id);

        if (notification is null)
            return;

        //temp***
        var notificationMessage = NotificationTemplate.RenderMessage(notification);

        foreach(var item in context.Message.channel)
        {
            var channel = (ChannelType)Enum.Parse(typeof(ChannelType), item);
            //temp***
            var message = Message.Create(NewId.NextGuid(),notification.Id, channel,Guid.NewGuid(),
                nameof(notification.NotificationType),notificationMessage,string.Empty,string.Empty);
            await _notificationDbContext.AddAsync(message);
        }
        await _notificationDbContext.SaveChangesAsync();

        await _publishEndpoint.Publish(new NotificationRendered(context.Message.Id));
    }
}
