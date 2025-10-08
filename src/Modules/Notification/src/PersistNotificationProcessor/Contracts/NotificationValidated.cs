using BuildingBlocks.Contracts;

namespace Notification.PersistNotificationProcessor.Contracts;
public record NotificationValidated(Guid Id, Guid UserId, Guid RequestId, NotificationType Type, 
    NotificationPriority Priority, string DataSchema);
