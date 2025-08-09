using BuildingBlocks.Exception;

namespace UserProfile.UserProfiles.Exceptions;

public class InvalidUserProfileIdException : AppException
{
    public InvalidUserProfileIdException(Guid Id) : base($"UserProfileId: {Id} is invalid")
    {
    }
}
