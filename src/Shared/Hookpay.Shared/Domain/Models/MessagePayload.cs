using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Domain.Models;

public class MessagePayload
{
    public int entity_id { get; set; }
    public PushType action { get; set; } = default;
    public string event_type { get; set; } = default!;
    public int user_id { get; set; }
    public Dictionary<string, object> detail { get; set; } = default!;
    public PriorityMessage priority { get; set; } = default;
}
public enum PushType
{
    InWeb = 0,
    Email = 1,
    Sms = 2,
}
public enum PriorityMessage 
{
    Low = 0,
    Medium = 1,
    High = 2,
}

