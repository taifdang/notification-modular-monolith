using BuildingBlocks.Core.Event;

namespace Notification.Notifications.Features.GettingNotificationById;

public record GetNotificationById();
public record PersonalNotificationCreatedDomainEvent(Guid Id, Guid UserId) : IDomainEvent;

