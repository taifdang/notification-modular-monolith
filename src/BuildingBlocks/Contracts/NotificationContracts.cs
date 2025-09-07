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

public record NotificationMessage(Guid MessageId, NotificationType NotificationType, ChannelType Channel, Recipient Recipient,
    object? MessageContent, Dictionary<string,object?> MetaData = null!);

public record Recipient(Guid UserId, string? Email);

public record NotificationCreated(Guid Id, Guid UserId, string Email) : IIntegrationEvent;
public record NotificationReadyToRender(Guid Id, Guid UserId, Guid RequestId, NotificationType Type, NotificationPriority Priority, string DataSchema,List<ChannelType> channel);
public record NotificationRendered(Guid Id,NotificationMessage NotificationMessage);

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
    InApp = 0,
    Email,
    Push,
}
public enum NotificationPriority
{
    Low = 0,
    Medium = 1,
    High = 2
}
