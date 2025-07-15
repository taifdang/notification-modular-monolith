using Hookpay.Shared.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Models;

public class OutboxMessage:Aggregate
{
    public int Id { get; set; }
    public int? UserId {  get; set; } 
    public Guid CorrelationId { get; set; }
    public int MessageId { get; set; }    
    public MessageType MessageType { get; set; }
    //public DateTime createdAt { get; set; }
    public MessageStatus Status { get; set; }
}
