using BuildingBlocks.Contracts;

namespace Notification.Feeds.Model;
public class UserFeed
{
    public Guid Id { get; set; }
    public Guid UserId {  get; set; }
    public NotificationType NotificationType { get; set; }
    public ChannelType ChannelType { get; set; }
    public string? MessageContent {  get; set; }
    public DateTime SentAt {  get; set; }

}
