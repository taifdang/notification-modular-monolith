using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Shared.Domain.Models;
using MassTransit.Futures.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Models;

public class Message : Aggregate
{  
    public int Id { get; set; }  
    public Guid CorrelationId { get; set; }
    public int UserId { get; set; }  
    public string Title { get; set; }
    public string Body { get; set; }
    public MessagePriority Priority { get; set; }
    public MessageType MessageType { get; set; }
    public bool IsProcessed { get; set; } = false;
    public static Message Create(
        Guid correlationId, 
        int userId ,
        string title, 
        string body      
    )
    {
        var message = new Message()
        {
            CorrelationId = correlationId,
            UserId = userId,
            Title = title,
            Body = body,          
            IsProcessed = false          
        };
        return message;
    }
    public void ChangeState(bool isProcessed)
    {
        IsProcessed = isProcessed;
    }

}
