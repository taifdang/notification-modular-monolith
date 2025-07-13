using MassTransit.Futures.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Models;

public class Message
{  
    public int mess_id { get; set; }  
    public Guid mess_correlationId { get; set; }
    public int mess_userId { get; set; }  
    public string mess_title { get; set; }
    public string mess_body { get; set; }
    public DateTime mess_createdAt { get; set; }
    public bool mess_processed { get; set; } = false;
    public static Message Create(Guid correlationId, int userId ,string title, string body,DateTime createdAt)
    {
        var message = new Message()
        {
            mess_correlationId = correlationId,
            mess_userId = userId,
            mess_title = title,
            mess_body = body,
            mess_createdAt = createdAt,
            mess_processed = false
        };
        return message;
    }

}
