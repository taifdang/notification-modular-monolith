using Hookpay.Modules.Notifications.Core.Messages.Enums;

namespace Hookpay.Modules.Notifications.Core.Messages.Models;

public class MessagePayload
{
    public int EntityId { get; set; }
    public PushType PushType { get; set; } = default;
    public string EventType { get; set; } = default!;
    public int UserId { get; set; }
    public Dictionary<string, object> MetaData { get; set; } = default!;
    public MessagePriority Priority { get; set; } = default;
    public static int getWorkAt(MessagePriority? priority)
    {
        return priority switch { MessagePriority.High => 0, MessagePriority.Medium => 5, MessagePriority.Low => 10, _ => 0 };
    }
}
