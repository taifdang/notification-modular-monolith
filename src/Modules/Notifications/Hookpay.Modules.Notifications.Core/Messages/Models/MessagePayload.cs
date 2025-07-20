using Hookpay.Modules.Notifications.Core.Messages.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Models;

public class MessagePayload
{
    public int entity_id { get; set; }
    public PushType action { get; set; } = default;
    public string event_type { get; set; } = default!;
    public int user_id { get; set; }
    public Dictionary<string, object> detail { get; set; } = default!;
    public MessagePriority priority { get; set; } = default;
    public static int getWorkAt(MessagePriority? priority)
    {
        return priority switch { MessagePriority.High => 0, MessagePriority.Medium => 5, MessagePriority.Low => 10, _ => 0 };
    }
}
