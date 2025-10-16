
using BuildingBlocks.Contracts;

namespace Notification.Infrastructure.Messages.Models;

//Id = notificationId
public record NotificationMessage(Guid NotificationId, Guid CorrelationId, NotificationType NotificationType, ChannelType Channel, Recipient Recipient,
    object? Message, IDictionary<string, object?>? Metadata = null);
