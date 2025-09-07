using BuildingBlocks.Contracts;

namespace Notification.Templates.Model;

public record Template
{
    public Guid Id { get; set; }
    public NotificationType NotificationType { get; set; } = default!;
    public ChannelType Channel {  get; set; }
    public string Name { get; set; }
    public string Content { get; set; } = default!; 
    public bool IsActive { get; set; }
    //public string? Variables { get; set; }
    //public LanguageType Language { get; set; } = default!;
}
