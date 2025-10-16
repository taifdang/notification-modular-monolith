
using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using BuildingBlocks.Core.CQRS;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Application.Common.Interfaces;
using Notification.Infrastructure.Messages.Contracts;
using System.Text.Json;

namespace Notification.Application.Notifications.Commands.IngrestNotification.Personal;

public record IngresNotificationPersonalCommand(Guid CorrelationId, NotificationType NotificationType, Recipient Recipient,
    IDictionary<string, object?> Payload, NotificationPriority Priority) : ICommand;
public class IngresNotificationPersonalCommandHandler : ICommandHandler<IngresNotificationPersonalCommand>
{
    private readonly INotificationDbContext _notificationDbContext;
    private readonly ILogger<IngresNotificationPersonalCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    public IngresNotificationPersonalCommandHandler(
        INotificationDbContext notificationDbContext,
        ILogger<IngresNotificationPersonalCommandHandler> logger,
        IPublishEndpoint publishEndpoint)
    {
        _notificationDbContext = notificationDbContext;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Unit> Handle(IngresNotificationPersonalCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(PersonalNotificationRequested));

        _logger.LogInformation($"consumer for {nameof(PersonalNotificationRequested)} is started");

        var notification = await _notificationDbContext.Notifications.SingleOrDefaultAsync(x => x.CorrelationId == command.CorrelationId);

        if (notification is null)
        {
            var notificationEntity = Domain.Entities.Notification.Create(
                NewId.NextGuid(),
                command.CorrelationId,
                command.NotificationType,
                JsonSerializer.Serialize(command),
                JsonSerializer.Serialize(command.Payload),
                command.Priority);

            await _notificationDbContext.Notifications.AddAsync(notificationEntity);

            await _notificationDbContext.SaveChangesAsync();

            await _publishEndpoint.Publish(new NotificationReceived(notificationEntity.Id, command.Recipient.UserId, command.Recipient.Email));
        }
        else
        {
            await _publishEndpoint.Publish(new NotificationReceived(notification.Id, command.Recipient.UserId, command.Recipient.Email));
        }

        

        return Unit.Value;
    }
}