using BuildingBlocks.Contracts;

namespace Notification.Recipents.Model;

public record Recipient
{  
    public Guid Id { get; set; }
    public Guid NotificationId { get; set; }
    public ChannelType Channel { get; set; }
    public Guid UserId { get; set; }
    public string? Target { get; set; }

    public static Recipient Create(Guid id, Guid notificationId, ChannelType channel, Guid userId, string? target)
    {
        var recipient = new Recipient
        {
            Id = id,
            NotificationId = notificationId,
            Channel = channel,
            UserId = userId,
            Target = target
        };

        return recipient;
    }
}
