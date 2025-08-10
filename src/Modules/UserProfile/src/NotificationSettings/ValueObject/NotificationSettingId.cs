using UserProfile.NotificationSettings.Exceptions;

namespace UserProfile.NotificationSettings.ValueObject;

public class NotificationSettingId
{
    public Guid Value { get; }
    private NotificationSettingId(Guid value)
    {
        Value = value;
    }
    public static NotificationSettingId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidNotificationSettingIdException(value);
        }
        return new NotificationSettingId(value);
    }
    public static implicit operator Guid(NotificationSettingId notificationSettingId)
    {
        return notificationSettingId.Value;
    }
}
