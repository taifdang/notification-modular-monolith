using BuildingBlocks.Contracts;
using Notification.Scheduler.Enums;

namespace Notification.Scheduler.Model;

public class Scheduler
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public NotificationType NotificationType { get; set; }
    public ChannelType ChannelType {get; set; }
    public DateTime ScheduleTime { get; set; }
    public string? MessageContent { get; set; }  
    public SheduleStatus? Status { get; set; }
    public NotificationPriority Priority { get; set; }
    public int RetryLeft { get; set; } 
}
