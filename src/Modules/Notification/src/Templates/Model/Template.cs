using BuildingBlocks.Contracts;
using BuildingBlocks.Core.Model;
using Notification.Templates.Enums;

namespace Notification.Templates.Model;

public record Template : Aggregate<Guid>
{
    public LanguageType Language { get; set; } = default!;
    public ChannelType Channel {  get; set; }
    public NotificationType NotificationType { get; set; } = default!;
    public string Content { get; set; } = default!;
    public bool IsActive { get; set; }  
}
