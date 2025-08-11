using BuildingBlocks.Exception;

namespace UserProfile.UserPreferences.Exceptions;

public class InvalidPreferenceException : AppException
{
    public InvalidPreferenceException() : base("Preference is invalid")
    {
    }
}
