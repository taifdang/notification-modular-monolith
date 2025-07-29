

using BuildingBlocks.Core.Event;
using BuildingBlocks.Core.Model;
using BuildingBlocks.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Data;

namespace BuildingBlocks.EFCore;

public class AppDbContextBase : DbContext, IDbContext
{
    private IDbContextTransaction _currentTransaction;
    private readonly ILogger<AppDbContextBase> _logger;
    private readonly ICurrentUserProvider _currentUserProvider;

    public AppDbContextBase(
        DbContextOptions options,
        ILogger<AppDbContextBase>? logger = null,
        ICurrentUserProvider? currentUserProvider = null)
        : base(options)
    {
        _logger = logger;
        _currentUserProvider = currentUserProvider;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        //ref: https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations#resolving-concurrency-conflicts
        catch(DbUpdateConcurrencyException ex)
        {
            foreach(var entry in ex.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync();

                if (databaseValues == null)
                {
                    _logger.LogWarning("The record no longer exists in the database, The record has been deleted by another user.");
                    throw;
                }

                // Refresh the original values
                entry.CurrentValues.SetValues(databaseValues);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
            return;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    public IExecutionStrategy CreateExcutionStragegy() => Database.CreateExecutionStrategy();

    //ref: https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency#execution-strategies-and-transactions
    public Task ExecutionTransactionAsync(CancellationToken cancellationToken = default)
    {
        var stategy = CreateExcutionStragegy();

        return stategy.ExecuteAsync(async () =>
        {
            await using var transaction =
                    await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

            try
            {
                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

        });
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        var domainEntites = ChangeTracker
            .Entries<IAggregate>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntites
            .SelectMany(x => x.DomainEvents)
            .ToImmutableList();
        
        domainEntites.ForEach(entity => entity.ClearDomainEvents());    

        return domainEvents.ToImmutableList();
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _currentTransaction?.RollbackAsync(cancellationToken)!;
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    // ref: https://www.meziantou.net/entity-framework-core-generate-tracking-columns.htm
    // ref: https://www.meziantou.net/entity-framework-core-soft-delete-using-query-filters.htm
    private void OnBeforeSaving()
    {
        try
        {
            foreach(var entry in ChangeTracker.Entries<IAggregate>())
            {
                var isAudit = entry.Entity.GetType().IsAssignableTo(typeof(IAggregate));
                var userId = _currentUserProvider?.GetCurrentUserId() ?? 0;

                if (isAudit)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedBy = userId;
                            entry.Entity.CreatedAt = DateTime.Now;
                            break;

                        case EntityState.Modified:
                            entry.Entity.UpdatedBy = userId;
                            entry.Entity.UpdatedAt = DateTime.Now;
                            entry.Entity.Version++;
                            break;

                        case EntityState.Deleted:
                            entry.Entity.UpdatedBy = userId;
                            entry.Entity.UpdatedAt = DateTime.Now;
                            entry.Entity.IsDeleted = true;
                            entry.Entity.Version++;
                            break;
                    }
                }

            }
        }
        catch(Exception ex) 
        {
            throw new Exception("Invalid, not find IAggregate ",ex);
        }
    }
}
