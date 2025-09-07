using BuildingBlocks.Contracts;

namespace Notification.NotificationLogs.Model;

public record NotificationLog
{
    public Guid Id { get; set; }
    public Guid NotificationId { get; set; }
    public ChannelType Channel { get; set; }
    public Enums.Status Status { get; set; }
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
            Status = Enums.Status.Pending,
            ErrorMessage = errorMessage,
            RetryCount = 0,
            SentAt = DateTime.Now,            
        };

        return log;
    }
    public void UpdateStatus(Enums.Status status, string? errorMessage = null)
    {
        Status = status;
        ErrorMessage = errorMessage;
        RetryCount += 1;
        if (status == Enums.Status.Sent)
        {
            SentAt = DateTime.Now;
        }
        else if (status == Enums.Status.Delivered)
        {
            DeliveredAt = DateTime.Now;
        }
    }
}
