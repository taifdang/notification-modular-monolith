using BuildingBlocks.Contracts;
using BuildingBlocks.Core.Model;
using Notification.Notifications.Enums;

namespace Notification.Notifications.Model;

public record Notification : Aggregate<Guid>
{
    public Guid RequestId { get; set; }
    public NotificationType NotificationType { get; set; }
    public string? MessageContent { get; set; } // root message
    public NotificationPriority Priority { get; set; }
    public NotificationStatus Status { get; set; }

    public static Notification Create(Guid Id, Guid RequestId, NotificationType NotificationType,string MessageContent,
        NotificationPriority Priority,bool isDeleted = false)
    {
        var notification = new Notification
        {
            Id = Id,
            RequestId = RequestId,
            NotificationType = NotificationType,
            MessageContent = MessageContent,
            Priority = Priority,
            Status = NotificationStatus.InProgress,
            IsDeleted = isDeleted
        };

        return notification;
    }
}
    