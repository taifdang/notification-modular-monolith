using BuildingBlocks.Core.Model;
using BuildingBlocks.PersistMessageProcessor;
using Notification.NotificationDeliveries.Enums;

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


    public static NotificationDelivery Create(Guid Id,Guid NotificationId,Guid UserId,
        BuildingBlocks.Contracts.NotificationType NotificationType,BuildingBlocks.Contracts.ChannelType ChannelType,
        string Message,BuildingBlocks.Contracts.NotificationPriority Priority,
        long RetryCount,bool IsDeleted = false)
    {
        var notificationDelivery = new NotificationDelivery
        {
            Id = Id,
            UserId = UserId,
            NotificationId = NotificationId,
            NotificationType = NotificationType,
            ChannelType = ChannelType,
            Message = Message,
            DeliveryStatus = Enums.DeliveryStatus.InProgress,
            Priority = Priority,
            RetryCount = RetryCount,
        };

        return notificationDelivery;
    }

    public void ChangeState(DeliveryStatus deliveryStatus)
    {
        DeliveryStatus = deliveryStatus;
    }
}
