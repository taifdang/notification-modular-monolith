using BuildingBlocks.Core.Model;

namespace Notification.Templates.Model;

public record Template : Aggregate<Guid>
{
    public string? Name { get; set; }
    public string? Content { get; set; }
    public bool IsActive { get; set; }  
}
