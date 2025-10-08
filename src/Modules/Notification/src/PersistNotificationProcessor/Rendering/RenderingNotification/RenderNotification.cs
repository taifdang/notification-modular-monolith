using HandlebarsDotNet;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Notification.Data;
using Notification.Extensions;
using Notification.PersistNotificationProcessor.Contracts;

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
        //NOTE: n event type + 1 or n channel = n template
        var recipients = await _notificationDbContext.Recipients
            .Where(x => x.NotificationId == context.Message.Id && x.UserId == context.Message.UserId)
            .ToListAsync();

        if (!recipients.Any())
        {
            return;
        }

        var channels = recipients.Select(r => r.Channel).Distinct().ToList();

        var templates = await _notificationDbContext.Templates
            .Where(x => x.NotificationType == context.Message.Type && channels.Contains(x.Channel))
            .ToListAsync();

        foreach (var recipient in recipients)
        {
            var template = templates.FirstOrDefault(t => t.Channel == recipient.Channel);

            if (template is null)
            {
                continue;
            }
           
            var messageContent = TemplateExtensions.RenderMessage(template, context.Message.DataSchema);

            var notificationMessage = new NotificationMessage(Guid.NewGuid(), context.Message.Type, recipient.Channel,
              new BuildingBlocks.Contracts.Recipient(context.Message.UserId, recipient.Target), messageContent,
              SetMetaData(context.Message.Priority.ToString(), template.Name, context.Message.RequestId.ToString()));

            await _publishEndpoint.Publish(new NotificationRendered(context.Message.Id, notificationMessage));
        }
    }
    private IDictionary<string, object> SetMetaData(string Priority, string Template, string RequestId)
    {
        var metadata = new Dictionary<string, object>();
        metadata.Add(nameof(Priority), Priority);
        metadata.Add(nameof(Template), Template);
        metadata.Add(nameof(RequestId), RequestId);

        return metadata;
    }
}
