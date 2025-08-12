
namespace BuildingBlocks.Contracts;

public class NotificationRequest
{
    public Guid Id { get; set; }
    public NotificationType NotificationType { get; set; }
    public List<string>? Channels { get; set; }
    public RecipientRequest Recipient { get; set; } = default!;
    public Dictionary<string, object?> Message { get; set; } = new Dictionary<string,object?>();
    public Dictionary<string, object?> Metadata { get; set; } = new Dictionary<string, object?>();

}

public class RecipientRequest 
{
    public Guid UserId { get; set; }
    public string? Email { get; set; }
}

public enum NotificationType
{
    UnKnown = 0,
    Promotion,
    Topup,
    Order,
    Transactional
}
public enum ChannelType
{
    None = 0,
    Email,
    Sms,
    Push,
    InApp
}
public enum NotificationPriority
{
    Low = 0,
    Medium = 1,
    High = 2
}
