using BuildingBlocks.Exception;

namespace Notification.Application.Common.Exceptions;

public class NotificationNotFoundException : DomainException
{
    public NotificationNotFoundException() : base("Notification not found")
    {
    }
}

