using BuildingBlocks.Exception;

namespace User.Profiles.Exceptions;

public class InvalidProfileIdException : AppException
{
    public InvalidProfileIdException(Guid Id) : base($"ProfileId: {Id} is invalid")
    {
    }
}
