using BuildingBlocks.Exception;

namespace Setting.NotificationPreferences.Exceptions;
public class InvalidNotificationPreferenceIdException : DomainException
{
    public InvalidNotificationPreferenceIdException(Guid Id) : base($"NotificationPreferenceId: {Id} is invalid")
    {
    }
}
