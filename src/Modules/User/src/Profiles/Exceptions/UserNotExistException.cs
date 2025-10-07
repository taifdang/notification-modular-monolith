using BuildingBlocks.Exception;

namespace User.Profiles.Exceptions;
public class UserNotExistException : AppException
{
    public UserNotExistException() : base($"Not found user")
    {
    }
}
