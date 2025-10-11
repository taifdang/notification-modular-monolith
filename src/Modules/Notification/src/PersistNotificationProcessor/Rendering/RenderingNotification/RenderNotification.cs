using Ardalis.GuardClauses;
using HandlebarsDotNet;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Data;
using Notification.Extensions;
using Notification.PersistNotificationProcessor.Contracts;
using Notification.Recipents.Model;

namespace Notification.PersistNotificationProcessor.Rendering.RenderingNotification;

public class RenderNotificationHandler : IConsumer<NotificationValidated>
{
    private readonly NotificationDbContext _notificationDbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RenderNotificationHandler> _logger;

    public RenderNotificationHandler(NotificationDbContext notificationDbContext, IPublishEndpoint publishEndpoint, ILogger<RenderNotificationHandler> logger)
    {
        _notificationDbContext = notificationDbContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<NotificationValidated> context)
    {
        Guard.Against.Null(context.Message, nameof(NotificationValidated));

        _logger.LogInformation($"consumer for {nameof(NotificationValidated)} is started");

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
            string messageContent;

            if (template is null)
            {
                //fallback to raw data schema if no template found
                _logger.LogWarning("No template found for NotificationType {NotificationType} and Channel {Channel}",
                    context.Message.Type, recipient.Channel);
                messageContent = context.Message.DataSchema;
            }
            else
            {
                messageContent = TemplateExtensions.RenderMessage(template, context.Message.DataSchema);
            }

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
