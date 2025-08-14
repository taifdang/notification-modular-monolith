using BuildingBlocks.Exception;

namespace UserProfile.UserProfiles.Exceptions;

public class InvalidAgeException : AppException
{
    public InvalidAgeException() : base("Age is not a negative value")
    {
    }
}
