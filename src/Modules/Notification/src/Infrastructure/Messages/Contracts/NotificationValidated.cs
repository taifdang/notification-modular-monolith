using BuildingBlocks.Contracts;

namespace Notification.Infrastructure.Messages.Contracts;

public record NotificationValidated(Guid CorrelationId, Guid NotificationId, Guid UserId, NotificationType Type,
    NotificationPriority Priority, string? Payload);
