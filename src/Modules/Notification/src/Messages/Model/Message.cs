using BuildingBlocks.Contracts;

namespace Notification.Messages.Model;
public class Message
{
    public Guid Id { get; set; }
    public Guid NotificationId { get; set; } //FK
    public ChannelType ChannelType { get; set; }
    public string? Subject {  get; set; }
    public string Body { get; set; } = default!;
    public string? Attachments { get; set; }
    public string? Extra { get; set; }  
}
