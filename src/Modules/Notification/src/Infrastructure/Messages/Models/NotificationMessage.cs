
using BuildingBlocks.Contracts;

namespace Notification.Infrastructure.Messages.Models;

public record NotificationMessage(Guid CorrelationId, Guid NotificationId, NotificationType NotificationType, ChannelType Channel, Recipient Recipient,
    object? Message, IDictionary<string, object?>? Metadata = null);
