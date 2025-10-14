namespace Notification.PersistNotificationProcessor.Contracts;
public record NotificationRendered(Guid Id, Guid RequestId, NotificationDispatched NotificationMessage, string dataSchema);
