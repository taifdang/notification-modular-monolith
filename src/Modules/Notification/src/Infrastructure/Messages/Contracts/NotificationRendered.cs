
using Notification.Infrastructure.Messages.Models;

namespace Notification.Infrastructure.Messages.Contracts;

public record NotificationRendered(Guid NotificationId, Guid CorrrelationId, NotificationMessage NotificationMessage, string Payload);
