using BuildingBlocks.Exception;

namespace UserProfile.UserProfiles.Exceptions;
public class UserIdNotExistException : AppException
{
    public UserIdNotExistException(Guid Id) : base($"UserId :{Id} is invalid")
    {
    }
}
