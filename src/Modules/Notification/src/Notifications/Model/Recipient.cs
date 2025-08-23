namespace Notification.Notifications.Model;

public record Recipient
{
    public Recipient(Guid id, Guid notificationId, Guid userId, string? target)
    {
        Id = id;
        NotificationId = notificationId;
        UserId = userId;
        Target = target;
    }

    public Guid Id { get; set; }
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
    public string? Target {  get; set; }
}
