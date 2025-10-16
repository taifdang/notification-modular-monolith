using BuildingBlocks.Contracts;

namespace Notification.Infrastructure.Messages.Contracts;

public record NotificationValidated(Guid NotificationId, Guid CorrelationId, Guid UserId, NotificationType Type,
    NotificationPriority Priority, string Metadata);
