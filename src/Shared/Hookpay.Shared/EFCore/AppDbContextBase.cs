using Hookpay.Shared.Core.Events;
using Hookpay.Shared.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Immutable;
using System.Data;


namespace Hookpay.Shared.EFCore;

public class AppDbContextBase : DbContext, IDbContext
{
    private  IDbContextTransaction _transaction;  
    protected AppDbContextBase(DbContextOptions options):base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null) return;
        _transaction = await  Database.BeginTransactionAsync(IsolationLevel.ReadCommitted,cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            await _transaction?.CommitAsync(cancellationToken)!;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public IExecutionStrategy CreateExcuteStrategy() => Database.CreateExecutionStrategy();

    public Task ExecuteTransactionAsync(CancellationToken cancellationToken = default)
    {
        var strategy = CreateExcuteStrategy();
        return strategy.ExecuteAsync( async () =>
        {
            await using var transaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted,cancellationToken);
            try
            {
                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
            }
        });
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _transaction?.RollbackAsync(cancellationToken)!;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch(DbUpdateConcurrencyException ex)
        {
            foreach(var entry in ex.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

                if (databaseValues is null) throw;

                // Refresh the original values to bypass next concurrency check
                entry.CurrentValues.SetValues(databaseValues);
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
       
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        var domainEntities = ChangeTracker
            .Entries<IAggregate>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToImmutableList();

        domainEntities.ForEach(entity => entity.ClearDomainEvent());

        return domainEvents.ToImmutableList();
    }
    public void OnBeforeSaving()
    {
        var userId = GetCurrentUser();
        try
        {
            foreach (var entry in ChangeTracker.Entries<IAggregate>())
            {
                var isAuditable = entry.Entity.GetType().IsAssignableTo(typeof(IAggregate));

                if (isAuditable)
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
                            entry.State = EntityState.Modified;
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
            throw new Exception("try for find IAggregate", ex);
        }
    }
    public int GetCurrentUser()
    {
        return 1;
    }
}
