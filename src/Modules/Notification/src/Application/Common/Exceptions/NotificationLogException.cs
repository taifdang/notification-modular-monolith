
using BuildingBlocks.Exception;

namespace Notification.Application.Common.Exceptions;

public class NotificationLogException : DomainException
{
    public NotificationLogException() : base("NotificationLog not found")
    {
    }
}