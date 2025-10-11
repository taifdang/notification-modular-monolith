using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Signalr;

public interface IConnectionDbContext
{
    DbSet<Connection> Connection { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task ExecuteTransactionAsync(CancellationToken cancellationToken = default);
}
