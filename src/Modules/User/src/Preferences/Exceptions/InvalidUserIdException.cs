using BuildingBlocks.Exception;

namespace User.Preferences.Exceptions;
public class InvalidUserIdException : DomainException
{
    public InvalidUserIdException(Guid Id) : base($"UserId: {Id} is invalid")
    {
    }
}
