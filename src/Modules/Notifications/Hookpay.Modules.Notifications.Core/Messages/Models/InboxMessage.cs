using Hookpay.Shared.Contracts;
using Hookpay.Shared.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Models;

public class InboxMessage:Aggregate
{ 
    public Guid CorrelationId { get; set; } 
    public string EventType {  get; set; }
    public string Payload { get; set; }
    //public DateTime createdAt { get; set; }
    public bool IsProcessed { get; set; }
    public static InboxMessage Create(Guid correlationId, string eventType, string payload)
    {
        var inboxMessage = new InboxMessage
        {
            CorrelationId = correlationId,
            EventType = eventType,
            Payload = payload,
            //createdAt = DateTime.UtcNow,
            IsProcessed = true
        };
        //inboxMessage.AddDomainEvent();
        return inboxMessage;
    }

}
