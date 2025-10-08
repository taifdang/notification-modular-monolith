using BuildingBlocks.Contracts;

namespace Notification.PersistNotificationProcessor.Contracts;
public record NotificationRendered(Guid Id, NotificationMessage NotificationMessage);
