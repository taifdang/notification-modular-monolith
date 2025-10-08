using BuildingBlocks.Contracts;
using HandlebarsDotNet;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notification.Data;
using Notification.PersistNotificationProcessor.Contracts;
using System.Text.Json;

namespace Notification.PersistNotificationProcessor.Rendering.RenderingNotification;

public class RenderNotificationHandler : IConsumer<NotificationValidated>
{
    private readonly NotificationDbContext _notificationDbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    public RenderNotificationHandler(NotificationDbContext notificationDbContext, IPublishEndpoint publishEndpoint)
    {
        _notificationDbContext = notificationDbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<NotificationValidated> context)
    {
        var recipient = await _notificationDbContext.Recipients
            .Where(x => x.NotificationId == context.Message.Id && x.UserId == context.Message.UserId)
            .ToListAsync();

        if (!recipient.Any())
        {
            return;
        }

        //NOTE: more event type, one channel -> more template -> need to select template base on event type and channel
        //currently: one event type = notification type, one channel -> one template

        var channels = recipient.Select(x => x.Channel);

        if (!channels.OrderBy(x => x).SequenceEqual(context.Message.channel.OrderBy(x => x)))
        {
            return;
        }

        foreach (var item in recipient)
        {
            var template = await _notificationDbContext.Templates
              .SingleOrDefaultAsync(x => x.NotificationType == context.Message.Type && x.Channel == item.Channel);

            if (template is null)
            {
                throw new Exception($"Not found template for notification type {context.Message.Type} and channel {item.Channel}");
            }

            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(context.Message.DataSchema);
            var compiledTemplate = Handlebars.Compile(template.Content);
            var messageContent = compiledTemplate(data);

            var notificationMessage = new Contracts.NotificationMessage(
               MessageId: Guid.NewGuid(),
               NotificationType: context.Message.Type,
               Channel: item.Channel,
               Recipient: new Recipient(context.Message.UserId, item.Target),
               MessageContent: messageContent,
               MetaData: new Dictionary<string, object?>
               {
                    { "Priority", context.Message.Priority.ToString() },
                    { "Template", template.Name },
                    { "RequestId", context.Message.RequestId.ToString() }
               });

            await _publishEndpoint.Publish(new NotificationRendered(context.Message.Id, notificationMessage));
        }
    }
}
