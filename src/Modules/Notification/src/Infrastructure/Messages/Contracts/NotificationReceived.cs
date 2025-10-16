
namespace Notification.Infrastructure.Messages.Contracts;

public record NotificationReceived(Guid NotificationId, Guid UserId, string Email);
