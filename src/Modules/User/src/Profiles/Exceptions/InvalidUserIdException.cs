using BuildingBlocks.Exception;

namespace User.Profiles.Exceptions;

public class InvalidUserIdException : AppException
{
    public InvalidUserIdException(Guid Id) : base($"UserId: {Id} is invalid")
    {
    }
}
