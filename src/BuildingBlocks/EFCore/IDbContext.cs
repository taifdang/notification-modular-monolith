
using BuildingBlocks.Core.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BuildingBlocks.EFCore;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity> () 
        where TEntity : class;
    IReadOnlyList<IDomainEvent> GetDomainEvents();
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync (CancellationToken cancellationToken = default);
    IExecutionStrategy CreateExcutionStragegy();
    Task ExecutionTransactionAsync(CancellationToken cancellationToken = default);
}
