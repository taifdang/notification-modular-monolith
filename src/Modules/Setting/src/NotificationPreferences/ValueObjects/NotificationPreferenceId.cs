using Setting.NotificationPreferences.Exceptions;

namespace Setting.NotificationPreferences.ValueObjects;

public record NotificationPreferenceId
{
    public Guid Value { get; }
    private NotificationPreferenceId(Guid value)
    {
        Value = value;
    }
    public static NotificationPreferenceId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidNotificationPreferenceIdException(value);
        }
        return new NotificationPreferenceId(value);
    }
    public static implicit operator Guid(NotificationPreferenceId userPreferenceId)
    {
        return userPreferenceId.Value;
    }
}

