namespace Notification.Recipients.Model;
public class Recipient
{
    public Guid Id { get; set; }
    public Guid NotificationId { get; set; } //FK
    public Guid? UserId { get; set; }
    public string? Email { get; set; }

    public Recipient(Guid id, Guid notificationId, Guid? userId, string? email)
    {
        Id = id;
        NotificationId = notificationId;
        UserId = userId;
        Email = email;
    }
}
