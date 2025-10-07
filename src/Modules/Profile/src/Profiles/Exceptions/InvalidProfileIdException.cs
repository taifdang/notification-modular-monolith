using BuildingBlocks.Exception;

namespace Profile.Profiles.Exceptions;

public class InvalidProfileIdException : DomainException
{
    public InvalidProfileIdException(Guid Id) : base($"ProfileId: {Id} is invalid")
    {
    }
}
