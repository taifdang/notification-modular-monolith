using BuildingBlocks.Exception;

namespace UserProfile.UserProfiles.Exceptions;

public class InvalidEmailException : AppException
{
    public InvalidEmailException() : base("Email is not empty")
    {
    }
}
