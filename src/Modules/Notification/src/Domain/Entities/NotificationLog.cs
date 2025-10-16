using BuildingBlocks.Contracts;
using Notification.Domain.Enums;

namespace Notification.Domain.Entities;

public record NotificationLog
{
    public Guid Id { get; set; }
    public Guid NotificationId { get; set; }
    public ChannelType Channel { get; set; }
    public DeliveryStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public static NotificationLog Create(Guid id, Guid notificationId, ChannelType channel, string? errorMessage = null)
    {
        var log = new NotificationLog
        {
            Id = id,
            NotificationId = notificationId,
            Channel = channel,
            Status = DeliveryStatus.Pending,
            ErrorMessage = errorMessage,
            RetryCount = 0,
            SentAt = DateTime.Now,
        };

        return log;
    }
    public void ChangeStatus(DeliveryStatus status, string? errorMessage = null)
    {
        Status = status;
        ErrorMessage = errorMessage;
        RetryCount += 1;
        if (status == DeliveryStatus.Sent)
        {
            SentAt = DateTime.Now;
        }
        else if (status == DeliveryStatus.Delivered)
        {
            DeliveredAt = DateTime.Now;
        }
    }
}
