using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Data;
using Notification.PersistNotificationProcessor.Contracts;
using System.Text.Json;

namespace Notification.PersistNotificationProcessor.Ingestion.Personal.IngestingPersonalNotification;
public class IngestePersonalNotificationHandler : IConsumer<PersonalNotificationRequested>
{
    private readonly NotificationDbContext _notificationDbContext;
    private readonly ILogger<IngestePersonalNotificationHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public IngestePersonalNotificationHandler(NotificationDbContext notificationDbContext, 
        ILogger<IngestePersonalNotificationHandler> logger, IPublishEndpoint publishEndpoint)
    {
        _notificationDbContext = notificationDbContext;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }
    public async Task Consume(ConsumeContext<PersonalNotificationRequested> context)
    {
        Guard.Against.Null(context.Message, nameof(PersonalNotificationRequested));

        _logger.LogInformation($"consumer for {nameof(PersonalNotificationRequested)} is started");

        var notification = await _notificationDbContext.Notifications.SingleOrDefaultAsync(x => x.RequestId == context.Message.RequestId);

        if (notification is  null)
        {
            var notificationEntity = Notifications.Model.Notification.Create(
            NewId.NextGuid(), context.Message.RequestId, context.Message.NotificationType,
            JsonSerializer.Serialize(context.Message), JsonSerializer.Serialize(context.Message.Payload),
            context.Message.Priority);

            await _notificationDbContext.Notifications.AddAsync(notificationEntity);

            await _notificationDbContext.SaveChangesAsync();
        }

        await _publishEndpoint.Publish(
            new NotificationReceived(notification.Id, context.Message.Recipient.UserId, context.Message.Recipient.Email));
    }
}
