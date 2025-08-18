using BuildingBlocks.Core.Model;

namespace Notification.NotificationDeliveries.Model;
public record NotificationDelivery : Aggregate<Guid>
{
    public Guid NotificationId { get; set; }
    public Guid UserId { get; set; }
    public BuildingBlocks.Contracts.NotificationType NotificationType { get; set; }
    public BuildingBlocks.Contracts.ChannelType ChannelType { get; set; }
    public string Message { get; set; } = default!;
    public Enums.DeliveryStatus DeliveryStatus { get; set; }
    public BuildingBlocks.Contracts.NotificationPriority Priority { get; set; }
    public long RetryCount { get; set; }
}
