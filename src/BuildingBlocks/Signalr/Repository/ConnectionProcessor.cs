using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Signalr.Repository;

public class ConnectionProcessor : IConnectionProcessor
{
    private readonly IConnectionDbContext _connectionDbContext;
    private readonly IHubContext<SignalrHub> _hubContext;

    public ConnectionProcessor(IConnectionDbContext connectionDbContext, IHubContext<SignalrHub> hubContext)
    {
        _connectionDbContext = connectionDbContext;
        _hubContext = hubContext;
    }

    public async Task AddConnectionAsync(Guid userId, string connectionId, long? tokenExpiry = null, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionDbContext.Connection.
            FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.ConnectionId == connectionId,
                cancellationToken);

        if (connection is not null)
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

    public async Task AddConnectionInGroupAsync(Guid userId, string groupName, CancellationToken cancellationToken = default)
    {
        var listConnection = await _connectionDbContext.Connection
             .Where(x => x.UserId == userId && x.IsConnected)
             .Select(x => x.ConnectionId)
             .ToListAsync(cancellationToken);

        foreach (var connectionId in listConnection)
        {
           await _hubContext.Groups.AddToGroupAsync(connectionId, groupName, cancellationToken);
        }

    }
}
