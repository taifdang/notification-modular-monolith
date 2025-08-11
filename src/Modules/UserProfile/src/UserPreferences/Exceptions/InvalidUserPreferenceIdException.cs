using BuildingBlocks.Exception;

namespace UserProfile.UserPreferences.Exceptions;
public class InvalidUserPreferenceIdException : AppException
{
    public InvalidUserPreferenceIdException(Guid Id) : base($"UserPreferenceId: {Id} is invalid")
    {
    }
}
