using BuildingBlocks.Contracts;

namespace Notification.PersistNotificationProcessor.Contracts;

public record NotificationMessage(Guid MessageId, NotificationType NotificationType, ChannelType Channel, Recipient Recipient,
    object? Message, IDictionary<string, object?>? MetaData = null);

