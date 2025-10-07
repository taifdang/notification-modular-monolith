using BuildingBlocks.Exception;

namespace User.Profiles.Exceptions;

public class InvalidUserNameException : AppException
{
    public InvalidUserNameException() : base("Name is not empty")
    {
    }
}
