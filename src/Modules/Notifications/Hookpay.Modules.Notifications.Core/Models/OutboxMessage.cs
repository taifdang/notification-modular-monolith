using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Models;

public class OutboxMessage
{
    public int id { get; set; }
    public string userId {  get; set; } 
    public Guid correlationId { get; set; }
    public string title { get; set; }   
    public DateTime createdAt { get; set; }
    public MessageStatus status { get; set; }
}
