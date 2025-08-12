using BuildingBlocks.Contracts;

namespace Notification.Notifications.Model;

public record Notification 
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public NotificationType NotificationType { get; set; }
    public string? MessageContent { get; set; } // root message
    public NotificationPriority Priority { get; set; }
    public string? SentBy { get; set; }
    public DateTime ScheduleAt { get; set; }
    public int Retries { get; set; }
    public Enums.NotificationStatus Status { get; set; }
}
    