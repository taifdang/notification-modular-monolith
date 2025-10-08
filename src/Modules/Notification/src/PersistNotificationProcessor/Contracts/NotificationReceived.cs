namespace Notification.PersistNotificationProcessor.Contracts;
public record NotificationReceived(Guid Id, Guid UserId, string Email);

