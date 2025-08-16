using BuildingBlocks.Core.Event;
using MassTransit;

namespace BuildingBlocks.Contracts;

public record PersonalNotificationCreated(NotificationType NotificationType,Recipient Recipient, Dictionary<string, object?> Payload, 
    NotificationPriority Priority) : IIntegrationEvent
{
    public Guid RequestId { get; init; } = NewId.NextGuid();
}
public record BroadcastNotificationCreated(NotificationType NotificationType,Dictionary<string, object?> Payload, NotificationPriority Priority)
    : IIntegrationEvent
{
    public Guid RequestId { get; init; } = NewId.NextGuid();
}

public record Recipient(Guid UserId,string? Email);

public enum NotificationType
{
    UnKnown = 0,
    Promotion,
    Topup,
    Order,
    Transactional
}
[Flags]
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
