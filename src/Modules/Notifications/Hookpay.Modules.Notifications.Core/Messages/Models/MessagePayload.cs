using Hookpay.Modules.Notifications.Core.Messages.Enums;

namespace Hookpay.Modules.Notifications.Core.Messages.Models;

public class MessagePayload
{
    public int EntityId { get; set; } //ex: userId
    public string? Target { get; set; } //ex: deviceToken/userId/Email
    public string? EventType { get; set; }
    public PushType PushType { get; set; } = default;
    public Dictionary<string, object?>? Data { get; set; } = new Dictionary<string, object?>();
    public MessagePriority Priority { get; set; } = default;

    public static int getWorkAt(MessagePriority? priority)
    {
        return priority switch 
        {   
            MessagePriority.High => 0,
            MessagePriority.Medium => 5, 
            MessagePriority.Low => 10, _ => 0 
        };
    }
}
