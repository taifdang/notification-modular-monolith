namespace Notification.Messages.Model;
public class Message
{
    public Message(Guid id, Guid notificationId, string key, string value)
    {
        Id = id;
        NotificationId = notificationId;
        Key = key;
        Value = value;
    }

    public Guid Id { get; set; }
    public Guid NotificationId { get; set; } //FK
    public string Key { get; set; }
    public string Value { get; set; }
}
