using BuildingBlocks.Exception;

namespace UserProfile.UserProfiles.Exceptions;

public class InvalidUserIdException : AppException
{
    public InvalidUserIdException(Guid Id) : base($"UserId: {Id} is invalid")
    {
    }
}
