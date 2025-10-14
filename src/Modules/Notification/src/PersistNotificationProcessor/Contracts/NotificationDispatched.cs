using BuildingBlocks.Contracts;

namespace Notification.PersistNotificationProcessor.Contracts;

public record NotificationDispatched(Guid Id, Guid RequestId, NotificationType NotificationType, ChannelType Channel, Recipient Recipient,
    object? Message, IDictionary<string, object?>? MetaData = null);

