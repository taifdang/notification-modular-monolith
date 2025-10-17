using BuildingBlocks.Contracts;

namespace Notification.Domain.Entities;

public record Notification
{
    public Guid Id { get; set; }
    public Guid CorrelationId { get; set; }
    public NotificationType NotificationType { get; set; }
    public string Content { get; set; } = default!;
    public string? Payload { get; set; }
    public NotificationPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public static Notification Create(Guid id, Guid correlationId, NotificationType notificationType, string content,
       string payload, NotificationPriority priority)
    {
        var notification = new Notification
        {
            Id = id,
            CorrelationId = correlationId,
            NotificationType = notificationType,
            Content = content,
            Payload = payload,
            Priority = priority,
            CreatedAt = DateTime.UtcNow,
        };

        return notification;
    }
}
