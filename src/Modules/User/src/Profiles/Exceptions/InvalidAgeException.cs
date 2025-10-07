using BuildingBlocks.Exception;

namespace User.Profiles.Exceptions;
public class InvalidAgeException : AppException
{
    public InvalidAgeException() : base("Age is not a negative value")
    {
    }
}
