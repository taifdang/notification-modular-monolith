
namespace Notification.Infrastructure.Messages.Contracts;

public record NotificationReceived(Guid CorrelationId, Guid NotificationId, Guid UserId, string? Email);
