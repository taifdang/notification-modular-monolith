
using Hookpay.Shared.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Hookpay.Shared.PersistMessageProcessor;

public class PersistMessageDbContext : DbContext, IPersistMessageDbContext
{
    private readonly ILogger<PersistMessageDbContext> _logger;

    public PersistMessageDbContext(
        DbContextOptions<PersistMessageDbContext> options, 
        ILogger<PersistMessageDbContext> logger) 
        : base(options)
    {
        _logger = logger;
    }
    public DbSet<PersistMessage> PersistMessage => Set<PersistMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    //ref: https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency#execution-strategies-and-transactions
    public Task ExcuteTransactionalsync(CancellationToken cancellationToken = default)
    {
        var stategy = Database.CreateExecutionStrategy();
        return stategy.ExecuteAsync(async () =>
        {
            await using var transaction =  
            await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted , cancellationToken);

            try
            {
                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync();    
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
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
                var databaseValues = await entry.GetDatabaseValuesAsync();

                if(databaseValues == null)
                {
                    _logger.LogError("The record no exist in the database");
                    throw;
                }

                //Refresh
                entry.OriginalValues.SetValues(databaseValues);
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
        catch(Exception ex) 
        {
            throw new Exception("try for find IVersion", ex);
        }
    }

    public void CreateTableIfNotExist()
    {       
        string sqlCreateTable = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PersistMessage')
            BEGIN
                CREATE TABLE PersistMessage
                (
                    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
                    DataType NVARCHAR(100),
                    Data NVARCHAR(MAX),
                    Created DATETIME2 NOT NULL DEFAULT GETDATE(),
                    RetryCount INT NOT NULL DEFAULT 0,
                    MessageStatus TINYINT NOT NULL DEFAULT 1,
                    DeliveryType INT NOT NULL DEFAULT 1,
                    Version INT NOT NULL DEFAULT -1
                );
            END
        "; 

        Database.ExecuteSqlRaw(sqlCreateTable);
    }
}
