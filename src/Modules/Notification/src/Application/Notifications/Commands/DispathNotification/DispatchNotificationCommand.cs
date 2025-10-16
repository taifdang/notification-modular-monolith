
using Ardalis.GuardClauses;
using BuildingBlocks.Core.CQRS;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Common.Interfaces;
using Notification.Infrastructure.Messages.Contracts;
using Notification.Infrastructure.Messages.Models;

namespace Notification.Application.Notifications.Commands.DispathNotification;

public record DispatchNotificationCommand(Guid NotificationId, NotificationMessage NotificationMessage) : ICommand;
public class DispatchNotificationCommandHandler : ICommandHandler<DispatchNotificationCommand>
{
    private readonly INotificationDbContext _notificationDbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<DispatchNotificationCommandHandler> _logger;

    public DispatchNotificationCommandHandler(
        INotificationDbContext notificationDbContext, 
        IPublishEndpoint publishEndpoint,
        ILogger<DispatchNotificationCommandHandler> logger)
    {
        this._notificationDbContext = notificationDbContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<Unit> Handle(DispatchNotificationCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(DispatchNotificationCommand));

        _logger.LogInformation($"consumer for {nameof(DispatchNotificationCommand)} is started");

        var notificationLog = Domain.Entities.NotificationLog.Create(
            NewId.NextGuid(),
            command.NotificationId,
            command.NotificationMessage.Channel);

        await _notificationDbContext.NotificationLogs.AddAsync(notificationLog);

        await _notificationDbContext.SaveChangesAsync();

        await _publishEndpoint.Publish(new NotificationDispatched(notificationLog.Id, command.NotificationMessage), ctx =>
        {
            // transport: inmemory = skip, filter at consumer
            ctx.SetRoutingKey(command.NotificationMessage.Channel.ToString());
        });

        return Unit.Value;
    }
}

