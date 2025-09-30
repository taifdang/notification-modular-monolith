


namespace BuildingBlocks.Core.Model;

//ref: https://www.geeksforgeeks.org/c-sharp/c-sharp-abstract-classes/
public abstract record Entity<T> : IEntity<T>
{
    public T Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    public long Version { get ; set; }
}
