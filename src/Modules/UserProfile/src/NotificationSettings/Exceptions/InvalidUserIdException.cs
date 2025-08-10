using BuildingBlocks.Exception;
using System.Net;

namespace UserProfile.NotificationSettings.Exceptions;

public class InvalidUserIdException : AppException
{
    public InvalidUserIdException(Guid Id) : base($"UserId: {Id} is invalid")
    {
    }
}
