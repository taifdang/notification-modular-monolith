
using Notification.Infrastructure.Messages.Models;

namespace Notification.Infrastructure.Messages.Contracts;

public record NotificationDispatched(Guid NotificationLogId, NotificationMessage NotificationMessage);

