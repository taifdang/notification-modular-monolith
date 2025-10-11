using BuildingBlocks.Core.Event;
using MassTransit;

namespace BuildingBlocks.Contracts;

public record PersonalNotificationRequested(NotificationType NotificationType, Recipient Recipient, IDictionary<string, object?> Payload, 
    NotificationPriority Priority) : IIntegrationEvent
{
    public Guid RequestId { get; init; } = NewId.NextGuid();
}
public record BroadcastNotificationRequested(NotificationType NotificationType, IDictionary<string, object?> Payload, NotificationPriority Priority)
    : IIntegrationEvent
{
    public Guid RequestId { get; init; } = NewId.NextGuid();
}
//state machine
public record NotificationSent(Guid TransactionId) : IIntegrationEvent;

public record Recipient(Guid UserId, string? Email);

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
