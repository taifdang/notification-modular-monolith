using BuildingBlocks.Exception;

namespace User.Profiles.Exceptions;
public class InvalidEmailException : AppException
{
    public InvalidEmailException() : base("Email is not empty")
    {
    }
}
