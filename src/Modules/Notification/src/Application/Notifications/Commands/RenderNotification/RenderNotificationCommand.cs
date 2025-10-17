
using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using BuildingBlocks.Core.CQRS;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Application.Common.Exceptions;
using Notification.Application.Common.Interfaces;
using Notification.Infrastructure.Messages.Contracts;
using Notification.Infrastructure.Messages.Models;

namespace Notification.Application.Notifications.Commands.RenderNotification;

public record RenderNotificationCommand(Guid CorrelationId, Guid NotificationId, Guid UserId, NotificationType Type,
    NotificationPriority Priority, string? Payload) : ICommand;

public class RenderNotificationCommandHandler : ICommandHandler<RenderNotificationCommand>
{
    private readonly INotificationDbContext _notificationDbContext;
    private readonly ILogger<RenderNotificationCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public RenderNotificationCommandHandler(
        INotificationDbContext notificationDbContext, 
        ILogger<RenderNotificationCommandHandler> logger, 
        IPublishEndpoint publishEndpoint)
    {
        _notificationDbContext = notificationDbContext;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }


    public async Task<Unit> Handle(RenderNotificationCommand command, CancellationToken cancellationToken)
    {
        //NON_IMPLEMENT: MEDATA NULL OR EMPTY ???
        Guard.Against.Null(command, nameof(RenderNotificationCommand));

        _logger.LogInformation($"consumer for {nameof(RenderNotificationCommand)} is started");

        //note: n event type + 1 or n channel = n template
        var recipients = await _notificationDbContext.Recipients
            .Where(x => x.NotificationId == command.NotificationId && x.UserId == command.UserId)
            .ToListAsync();

        if (!recipients.Any()) throw new RecipientNotFoundException();

        var channels = recipients.Select(r => r.Channel).Distinct().ToList();

        var templates = await _notificationDbContext.Templates
            .Where(x => x.NotificationType == command.Type && channels.Contains(x.Channel))
            .ToListAsync();

        foreach (var recipient in recipients)
        {
            var template = templates.FirstOrDefault(t => t.Channel == recipient.Channel);
            string messageContent;

            if (template is null)
            {
                //fallback to raw data schema if no template found
                _logger.LogWarning("No template found for NotificationType {NotificationType} and Channel {Channel}",
                    command.Type, recipient.Channel);
                messageContent = command.Payload;
            }
            else
            {
                messageContent = TemplateExtensions.RenderMessage(template, command.Payload);
            }

            var notificationMessage = new NotificationMessage(
                command.CorrelationId,
                command.NotificationId,              
                command.Type,
                recipient.Channel, 
                new BuildingBlocks.Contracts.Recipient(
                   command.UserId, 
                   recipient.Target),
                messageContent,
                SetMetaData(
                   command.Priority.ToString(),
                   template.Name, 
                   command.CorrelationId.ToString()));

            await _publishEndpoint.Publish(new NotificationRendered(notificationMessage));
        }

        return Unit.Value;
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
