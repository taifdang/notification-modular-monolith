using BuildingBlocks.Core.Event;
using MassTransit;

namespace BuildingBlocks.Contracts;

public record PersonalNotificationRequested(NotificationType NotificationType,Recipient Recipient, Dictionary<string, object?> Payload, 
    NotificationPriority Priority) : IIntegrationEvent
{
    public Guid RequestId { get; init; } = NewId.NextGuid();
}
public record BroadcastNotificationRequested(NotificationType NotificationType,Dictionary<string, object?> Payload, NotificationPriority Priority)
    : IIntegrationEvent
{
    public Guid RequestId { get; init; } = NewId.NextGuid();
}

public record NotificationMessage(ChannelType ChannelType);

public record Recipient(Guid UserId, string? Email);


public record NotificationCreated(Guid Id, Guid UserId) : IIntegrationEvent;
public record NotificationReadyToRender(Guid Id, List<string> channel);
public record NotificationRendered(Guid Id);


public enum NotificationType
{
    UnKnown = 0,
    Promotion,
    Topup,
    Order,
    Transactional,
    ChangePassword
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
