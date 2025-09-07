using BuildingBlocks.Contracts;

namespace Notification.Notifications.Model;

public record Notification
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public NotificationType NotificationType { get; set; }
    public string MessageContent { get; set; }
    public string? DataSchema { get; set; }
    public NotificationPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    //public NotificationStatus Status { get; set; }

    public static Notification Create(Guid Id, Guid RequestId, NotificationType NotificationType,string MessageContent,
        string DataSchema,NotificationPriority Priority)
    {
        var notification = new Notification
        {
            Id = Id,
            RequestId = RequestId,
            NotificationType = NotificationType,
            MessageContent = MessageContent,
            DataSchema = DataSchema,
            Priority = Priority,
            CreatedAt = DateTime.UtcNow,
            //Status = NotificationStatus.InProgress,
        };

        return notification;
    }
}
    