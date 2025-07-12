using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Models;

public class MessagePayload
{
    public int entity_id { get; set; }
    public PushType action { get; set; } = default;
    public string event_type { get; set; } = default!;
    public int user_id { get; set; }
    public Dictionary<string, object> detail { get; set; } = default!;//json tuy bien
    public PriorityMessage priority { get; set; } = default;// high|medium|low
                                                            //business rule
    public static int getWorkAt(PriorityMessage? priority)
    {
        return priority switch { PriorityMessage.High => 0, PriorityMessage.Medium => 5, PriorityMessage.Low => 10, _ => 0 };
    }
}
