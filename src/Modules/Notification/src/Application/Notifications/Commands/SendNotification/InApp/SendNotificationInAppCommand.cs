
using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Signalr;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Notification.Application.Common.Exceptions;
using Notification.Application.Common.Interfaces;
using Notification.Infrastructure.Messages.Models;

namespace Notification.Application.Notifications.Commands.SendNotification.InApp;

public record SendNotificationInAppCommand(Guid NotificationLogId, NotificationMessage NotificationMessage) : ICommand;
public class SendNotificationInAppCommandHandler : ICommandHandler<SendNotificationInAppCommand>
{
    private readonly ISignalrHub _signalrHub;
    private readonly ILogger<SendNotificationInAppCommandHandler> _logger;
    private readonly INotificationDbContext _notificationDbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public SendNotificationInAppCommandHandler(
        ISignalrHub signalrHub, 
        ILogger<SendNotificationInAppCommandHandler> logger,
        INotificationDbContext notificationDbContext,
        IPublishEndpoint publishEndpoint)
    {
        _signalrHub = signalrHub;
        _logger = logger;
        _notificationDbContext = notificationDbContext;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<Unit> Handle(SendNotificationInAppCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(SendNotificationInAppCommand));

        _logger.LogInformation($"consumer for {nameof(SendNotificationInAppCommand)} is started");

        var @event = command.NotificationMessage;

        //transport: inmemory = skip, filter at consumer
        if (@event.Channel != ChannelType.InApp)
        {
            return Unit.Value;
        }

        var notificationLog = await _notificationDbContext.NotificationLogs.FindAsync(command.NotificationLogId, cancellationToken);

        if (notificationLog == null) throw new NotificationLogException();

        try
        {
            await _signalrHub.ProcessAsync(@event.Recipient.UserId.ToString(), @event.Message.ToString());

            notificationLog.ChangeStatus(Domain.Enums.DeliveryStatus.Sent);

            await _notificationDbContext.SaveChangesAsync();

        }
        catch (Exception error)
        {         
            notificationLog.ChangeStatus(Domain.Enums.DeliveryStatus.Failed, error.ToString());

            await _notificationDbContext.SaveChangesAsync();           
        }

        return Unit.Value;
    }
}

