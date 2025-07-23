
using Microsoft.EntityFrameworkCore;

namespace Hookpay.Shared.PersistMessageProcessor;

public interface IPersistMessageDbContext
{
    DbSet<PersistMessage> PersistMessage { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task ExcuteTransactionalsync(CancellationToken cancellationToken = default);
}
