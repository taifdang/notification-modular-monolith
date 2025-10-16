using BuildingBlocks.Contracts;

namespace Notification.Domain.Entities;

public record Template
{
    public Guid Id { get; set; }
    public NotificationType NotificationType { get; set; } = default!;
    public ChannelType Channel { get; set; }
    public string Name { get; set; }
    public string Content { get; set; } = default!;
    public bool IsActive { get; set; }
}
