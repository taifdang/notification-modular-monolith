using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record PersonalNotificationRequested(Guid CorrelationId, NotificationType NotificationType, Recipient Recipient,IDictionary<string, object?> Payload, NotificationPriority Priority) : IIntegrationEvent;
public record BroadcastNotificationRequested(NotificationType NotificationType, IDictionary<string, object?> Payload, NotificationPriority Priority) : IIntegrationEvent;
public record NotificationSentEvent(Guid TransactionId) : IIntegrationEvent;
//Step could be "Receive", "Validate", "Render", "Dispatch", "Send"
public record NotificationFailedEvent(Guid TransactionId, Guid? NotificationId, string Step, string ErrorMessage) : IIntegrationEvent;

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
