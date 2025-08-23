using BuildingBlocks.Contracts;
using BuildingBlocks.Core.Model;
using Notification.Notifications.Enums;

namespace Notification.Notifications.Model;
public record Message : Aggregate<Guid>
{
    public Guid NotificationId { get; set; }
    public ChannelType Channel {  get; set; }
    public Guid? TemplateId { get; set; }
    public string? Subject { get; set; }
    public string Body { get; set; } = default!;
    public string? Attachment {  get; set; }
    public string? Metadata { get; set; }    
    public MessageStatus Status { get; set; }

    public static Message Create(Guid Id,Guid NotificationId,ChannelType channelType,Guid TemplateId,string Subject,string Body,
        string Attachment,string Metadata)
    {
        var message = new Message
        {
            Id = Id,
            NotificationId = NotificationId,
            Channel = channelType,
            TemplateId = TemplateId,
            Subject = Subject,
            Body = Body,
            Attachment = Attachment,
            Metadata = Metadata,
            Status = MessageStatus.Inprogress
        };

        return message;
    }
}
