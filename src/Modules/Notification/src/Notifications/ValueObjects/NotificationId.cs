namespace Notification.Notifications.ValueObjects;
public class NotificationId
{
    public Guid Value { get; }
    private NotificationId(Guid value)
    {
        Value = value;
    }
    public static NotificationId Of(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new Exception("");
        }
        return new NotificationId(value);
    }
    public static implicit operator Guid(NotificationId notificationId)
    {
        return notificationId.Value;
    }
}
