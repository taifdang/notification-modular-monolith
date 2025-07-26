namespace Hookpay.Shared.Core.Model;

public interface IEntity
{    
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; } 
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}
