using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record NotificationCreated(NotificationEvent? message) 
    : IIntegrationEvent;

public record NotificationEvent
{
    public Guid requestId { get; set; }
    public NotificationType notificationType { get; set; }
    public RecipientEvent recipient { get; set; } = new();
    public Dictionary<string,object?>? payload { get; set; }
    public MetadataEvent metadata { get; set; } = new();
}

public record RecipientEvent
{
    public Guid userId { get; set; }
    public string? email { get; set; }
}

public record MetadataEvent
{
    public NotificationPriority priority { get; set; }
    public int retries {  get; set; }
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
public enum EventType
{
    TopupCreated = 0,
    Promotion
}
