using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Signalr.Repository;

public class ConnectionProcessor : IConnectionProcessor
{
    private readonly IConnectionDbContext _connectionDbContext;

    public ConnectionProcessor(IConnectionDbContext connectionDbContext)
    {
        _connectionDbContext = connectionDbContext;
    }

    public async Task AddConnectionAsync(Guid userId, string connectionId, long? tokenExpiry = null, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionDbContext.Connection.
            FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.ConnectionId == connectionId,
                cancellationToken);

        if (connection is null)
            return;
        
        var connectionEntity = Connection.Create(userId, connectionId, tokenExpiry);

        await _connectionDbContext.Connection.AddAsync(connectionEntity, cancellationToken);

        await _connectionDbContext.SaveChangesAsync(cancellationToken);
    } 

    public async Task RemoveConnectionAsync(Guid userId, string connectionId, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionDbContext.Connection.
            FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.ConnectionId == connectionId,
                cancellationToken);

        if (connection is null)
            return;

        connection.ChangeState(false);

        await _connectionDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<string>> GetConnectionAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _connectionDbContext.Connection
            .Where(x => x.UserId == userId)
            .Select(x => x.ConnectionId)
            .ToListAsync(cancellationToken);
    }
}
