using BuildingBlocks.Exception;

namespace Setting.NotificationPreferences.Exceptions;
public class NotificationPreferenceNotFoundException : DomainException
{
    public NotificationPreferenceNotFoundException() : base("NotificationPreference not found")
    {
    }
}
