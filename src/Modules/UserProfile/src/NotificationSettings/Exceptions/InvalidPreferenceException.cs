using BuildingBlocks.Exception;

namespace UserProfile.NotificationSettings.Exceptions;

public class InvalidPreferenceException : AppException
{
    public InvalidPreferenceException() : base("Preference is invalid")
    {
    }
}
