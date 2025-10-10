using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Configurations.Rules;
using Notification.Data;
using Notification.PersistNotificationProcessor.Contracts;
using User;

namespace Notification.PersistNotificationProcessor.Validation.ValidatingNotification;
public class ValidateNotification : IConsumer<NotificationReceived>
{
    private readonly NotificationDbContext _notificationDbContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly UserGrpcService.UserGrpcServiceClient _grpcClient;
    private readonly ILogger<ValidateNotification> _logger;

    public ValidateNotification(UserGrpcService.UserGrpcServiceClient grpcClient,
        NotificationDbContext notificationDbContext, IPublishEndpoint publishEndpoint, ILogger<ValidateNotification> logger)
    {
        _grpcClient = grpcClient;
        _notificationDbContext = notificationDbContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<NotificationReceived> context)
    {
        Guard.Against.Null(context.Message, nameof(NotificationReceived));

        _logger.LogInformation($"consumer for {nameof(NotificationReceived)} is started");

        var notification = await _notificationDbContext.Notifications.SingleOrDefaultAsync(x => x.Id == context.Message.Id);

        if (notification is null)
        {
            return;
        }

        var preferenceEntity = _grpcClient.GetPreferenceById(new GetPreferenceByIdRequest { Id = context.Message.UserId.ToString() });

        if (preferenceEntity is null)
        {
            return;
        }

        var preference = preferenceEntity.PreferenceDto.Preference
           .Select(p => new PreferenceDto((BuildingBlocks.Contracts.ChannelType)p.Channel, p.IsOptOut))
           .ToList();

        var channels = NotificationRule.GetChannels(notification, preference);

        if (!channels.Any())
        {
            throw new Exception("Occur error when get channels from user preference");
        }

        foreach (var channel in channels)
        {
            var recipient = Recipents.Model.Recipient.Create(NewId.NextGuid(), notification.Id,
                 channel, context.Message.UserId, context.Message.Email);

            await _notificationDbContext.Recipients.AddAsync(recipient);
        }
        await _notificationDbContext.SaveChangesAsync();

        await _publishEndpoint.Publish(new NotificationValidated(context.Message.Id, Guid.Parse(preferenceEntity.PreferenceDto.UserId),
            notification.RequestId, notification.NotificationType, notification.Priority, notification.DataSchema));
    }
}
