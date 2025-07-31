
using BuildingBlocks.Core.Event;
using BuildingBlocks.Core.Model;
using BuildingBlocks.EFCore;
using Identity.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using System.Reflection;

namespace Identity.Data;
public sealed class IdentityContext 
    : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,
    IDbContext

{
    private readonly ILogger<IdentityContext> _logger;
    private IDbContextTransaction _currentTransaction;

    public IdentityContext(
        DbContextOptions<IdentityContext> options,
        ILogger<IdentityContext> logger
        ) : base(options)
    {
        _logger = logger;
    }

    //ref: https://learn.microsoft.com/en-us/ef/core/querying/filters?tabs=ef10
    //ref: https://github.com/dotnet/efcore/issues/10275
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //builder.Entity<IAggregate>().HasQueryFilter(x => !x.IsDeleted);
        base.OnModelCreating(builder);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is not null)
            return;
        _currentTransaction = await Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            await _currentTransaction?.CommitAsync(cancellationToken)!;
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
        var strategy = CreateExcutionStragegy();
        return strategy.ExecuteAsync(async () =>
        {
            await using var transaction =
                await Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);

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
         var domainEntities = ChangeTracker
            .Entries<IAggregate>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x=>x.DomainEvents)
            .ToImmutableList();

        domainEntities.ForEach(x => x.ClearDomainEvents()); 

        return domainEvents.ToImmutableList();
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _currentTransaction?.RollbackAsync(cancellationToken)!;
        }
        catch
        {
            _currentTransaction?.Rollback();
            _currentTransaction = null;
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        //ref: https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations#resolving-concurrency-conflicts
        catch (DbUpdateConcurrencyException ex)
        {
            foreach(var entry in ex.Entries)
            {
                var databaseValues = entry.GetDatabaseValues();

                if (databaseValues == null)             
                {
                    _logger.LogInformation("The record no longer exists in the database, The record has been deleted by another user");
                    throw;
                }

                // Refresh the original values to bypass next concurrency check
                entry.CurrentValues.SetValues(databaseValues);  
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }

    private void OnBeforeSaving()
    {
        try
        {
            foreach (var entry in ChangeTracker.Entries<IVersion>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.Version++;
                        break;

                    case EntityState.Deleted:
                        entry.Entity.Version++;
                        break;
                }
            }
        }
        catch(System.Exception ex)
        {
            throw new Exception("try for find IVersion", ex);
        }
    }
}
