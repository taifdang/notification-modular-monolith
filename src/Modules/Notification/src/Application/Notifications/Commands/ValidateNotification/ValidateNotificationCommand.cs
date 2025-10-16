
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
using User;

namespace Notification.Application.Notifications.Commands.ValidateNotification;

public record ValidateNotificationCommand(Guid NotificationId, Guid UserId, string Email) : ICommand;
public class ValidateNotificationCommandHandler : ICommandHandler<ValidateNotificationCommand>
{
    private readonly INotificationDbContext _notificationDbContext;
    private readonly ILogger<ValidateNotificationCommandHandler> _logger;
    private readonly UserGrpcService.UserGrpcServiceClient _grpcClient;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRuleBaseService _ruleBaseService;
    public ValidateNotificationCommandHandler(
        INotificationDbContext notificationDbContext,
        ILogger<ValidateNotificationCommandHandler> logger,
        IPublishEndpoint publishEndpoint,
        UserGrpcService.UserGrpcServiceClient grpcClient,
        IRuleBaseService ruleBaseService)
    {
        _notificationDbContext = notificationDbContext;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _grpcClient = grpcClient;
        _ruleBaseService = ruleBaseService;
    }
    public async Task<Unit> Handle(ValidateNotificationCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(ValidateNotificationCommand));

        _logger.LogInformation($"consumer for {nameof(ValidateNotificationCommand)} is started");

        var notification = await _notificationDbContext.Notifications.SingleOrDefaultAsync(x => x.Id == command.NotificationId);

        if (notification is null) throw new NotificationNotFoundException();

        var preferenceEntity = _grpcClient.GetPreferenceById(new GetPreferenceByIdRequest { Id = command.UserId.ToString() });

        if (preferenceEntity is null) throw new PreferenceNotFoundException();

        var preference = preferenceEntity.PreferenceDto.Preference
           .Select(p => new PreferenceDto((BuildingBlocks.Contracts.ChannelType)p.Channel, p.IsOptOut))
           .ToList();

        var channels = _ruleBaseService.GetChannels(notification, preference);

        if (!channels.Any()) throw new ChannelNotFoundException();

        foreach (var channel in channels)
        {
            var recipient = Domain.Entities.Recipient.Create(
                NewId.NextGuid(),
                notification.Id,
                channel, 
                command.UserId,
                command.Email);

            await _notificationDbContext.Recipients.AddAsync(recipient);
        }
        await _notificationDbContext.SaveChangesAsync();

        await _publishEndpoint.Publish(new NotificationValidated(
            command.NotificationId,
            notification.CorrelationId,
            Guid.Parse(preferenceEntity.PreferenceDto.UserId),
            notification.NotificationType,
            notification.Priority, 
            notification.Metadata));

        return Unit.Value;
    }
}
