using BuildingBlocks.Exception;

namespace Setting.NotificationPreferences.Exceptions;
public class InvalidUserIdException : DomainException
{
    public InvalidUserIdException(Guid Id) : base($"UserId: {Id} is invalid")
    {
    }
}
