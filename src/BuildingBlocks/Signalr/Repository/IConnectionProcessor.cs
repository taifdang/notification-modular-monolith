namespace BuildingBlocks.Signalr.Repository;

public interface IConnectionProcessor
{
    public Task AddConnectionAsync(Guid userId, string connectionId, long? tokenExpiry = null, CancellationToken cancellationToken = default);
    public Task RemoveConnectionAsync(Guid UserId, string connectionId, CancellationToken cancellationToken = default);
    public Task<IEnumerable<string>> GetConnectionAsync(Guid userId, CancellationToken cancellationToken = default);
}
