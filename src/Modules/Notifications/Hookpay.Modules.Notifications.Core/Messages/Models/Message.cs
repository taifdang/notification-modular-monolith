using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Shared.Domain.Models;

namespace Hookpay.Modules.Notifications.Core.Messages.Models;

public class Message : Aggregate
{  
    public int Id { get; set; }  
    public Guid CorrelationId { get; set; }
    public int UserId { get; set; }  
    public string Title { get; set; }
    public string Body { get; set; }
    public string? MetaData { get; set; }
    public MessageType MessageType { get; set; }
    public Enums.PushType PushType { get; set; }
    public MessagePriority Priority { get; set; }
    public bool IsProcessed { get; set; }
    public bool IsRead { get; set; }
    public static Message Create(
        Guid correlationId, 
        int userId ,
        string title, 
        string body,
        string metaData
    )
    {
        var message = new Message()
        {
            CorrelationId = correlationId,
            UserId = userId,
            Title = title,
            Body = body,          
            IsProcessed = false,
            IsRead = false,
            MetaData = metaData

        };

        return message;
    }
    public void ChangeState(bool isProcessed)
    {
        IsProcessed = isProcessed;
    }
    public void ChangeReadState(bool isRead)
    {
        IsRead = isRead;
    }

}
