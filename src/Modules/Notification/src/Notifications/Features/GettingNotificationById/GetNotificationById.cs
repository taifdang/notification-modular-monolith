using BuildingBlocks.Core.Event;

namespace Notification.Notifications.Features.GettingNotificationById;

public record PersonalNotificationCreatedDomainEvent(Guid Id, Guid UserId, string email) : IDomainEvent;

