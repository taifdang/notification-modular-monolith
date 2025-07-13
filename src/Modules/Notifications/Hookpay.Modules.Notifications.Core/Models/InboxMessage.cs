using Hookpay.Shared.Contracts;
using Hookpay.Shared.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Models;

public class InboxMessage:Entity
{
   
    public Guid correlationId { get; set; } 
    public string eventType {  get; set; }
    public string payload { get; set; }
    public DateTime createdAt { get; set; }
    public bool processed { get; set; }
    public static InboxMessage Create(Guid correlationId, string eventType, string payload)
    {
        var inboxMessage = new InboxMessage
        {
            correlationId = correlationId,
            eventType = eventType,
            payload = payload,
            createdAt = DateTime.UtcNow,
            processed = true
        };
        //inboxMessage.AddDomainEvent();
        return inboxMessage;
    }

}
