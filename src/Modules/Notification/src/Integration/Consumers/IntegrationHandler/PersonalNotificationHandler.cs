using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Event;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Data;
using Notification.Notifications.Features.CreatingNotification;
using System.Text.Json;

namespace Notification.Integration.Consumers.IntegrationHandler;
public class PersonalNotificationHandler : IConsumer<PersonalNotificationRequested>
{
    private readonly NotificationDbContext _notificationDbContext;
    private readonly ILogger<PersonalNotificationHandler> _logger;
    private readonly IEventDispatcher _eventDispatcher;

    public PersonalNotificationHandler(NotificationDbContext notificationDbContext, ILogger<PersonalNotificationHandler> logger,
        IEventDispatcher eventDispatcher)
    {
        _notificationDbContext = notificationDbContext;
        _logger = logger;
        _eventDispatcher = eventDispatcher;
    }

    public async Task Consume(ConsumeContext<PersonalNotificationRequested> context)
    {
        //save notification
        var @event = context.Message;

        var notification = await _notificationDbContext.Notifications.SingleOrDefaultAsync(x => x.RequestId == @event.RequestId);

        if (notification is not null)
            return;

        //notification
        var notificationEntity = 
            Notifications.Model.Notification.Create(NewId.NextGuid(),@event.RequestId,@event.NotificationType,
                JsonSerializer.Serialize(@event.Payload),@event.Priority);

        _notificationDbContext.Notifications.Add(notificationEntity);

        //recipient
        _notificationDbContext.Recipients.Add(new Recipients.Model.Recipient(NewId.NextGuid(),notificationEntity.Id,
            @event.Recipient.UserId,@event.Recipient.Email));

        //message
        foreach(var item in @event.Payload)
            _notificationDbContext.Messages.Add(new Messages.Model.Message(NewId.NextGuid(),notificationEntity.Id,item.Key,
                JsonSerializer.Serialize(item.Value)));

        await _notificationDbContext.SaveChangesAsync();

        //internalcommand
        await _eventDispatcher.SendAsync(new PersonalNotificationCreatedDomainEvent(notificationEntity.Id), 
            typeof(IInternalCommand));

    }
}

public class NotificationIntegrationValidator : AbstractValidator<PersonalNotificationRequested>
{ 
    public NotificationIntegrationValidator()
    {
        RuleFor(x => x.NotificationType)
            .Must(r => r.GetType().IsEnum && r == NotificationType.Topup ||
                       r == NotificationType.Promotion ||
                       r == NotificationType.Order ||
                       r == NotificationType.Transactional ||
                       r == NotificationType.UnKnown)
            .WithMessage($"NotificationType is not supported");

        RuleFor(x => x.Priority)
            .Must(r => r == NotificationPriority.High ||
                       r == NotificationPriority.Medium ||
                       r == NotificationPriority.Low)
            .WithMessage($"Priority is not supported");
    }
}

