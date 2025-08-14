using BuildingBlocks.Exception;

namespace UserProfile.UserProfiles.Exceptions;
public class InvalidUserNameException : AppException
{
    public InvalidUserNameException() : base("Name is not empty")
    {
    }
}
