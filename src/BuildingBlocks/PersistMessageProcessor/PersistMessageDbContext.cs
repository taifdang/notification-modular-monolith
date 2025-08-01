

using BuildingBlocks.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.PersistMessageProcessor
{
    public class PersistMessageDbContext : DbContext, IPersistMessageDbContext
    {
        private readonly ILogger<PersistMessageDbContext> _logger;  

        public PersistMessageDbContext(
            DbContextOptions<PersistMessageDbContext> options,
            ILogger<PersistMessageDbContext> logger) : base (options)
        {
            _logger = logger;
        }

        public DbSet<PersistMessage> PersistMessage => Set<PersistMessage>();

        //ref: https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency#execution-strategies-and-transactions
        public Task ExecuteTransactionAsync(CancellationToken cancellationToken = default)
        {
            var stratery = Database.CreateExecutionStrategy();

            return stratery.ExecuteAsync(async () =>
            {
                await using var transaction =
                    await Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted ,cancellationToken);
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
                    var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

                    if (databaseValues is null)
                        _logger.LogWarning("");

                    // Refresh original values to bypass next concurrency check
                    entry.CurrentValues.SetValues(databaseValues);  
                }

                return await base.SaveChangesAsync(cancellationToken);
            }
        }

        private void OnBeforeSaving()
        {
            try
            {
                foreach(var entry in ChangeTracker.Entries<IVersion>())
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
                throw new System.Exception("not find IVersion,please try again", ex);
            }
        }

        public void CreateTableIfNotExist()
        {
            //ref: https://www.w3schools.com/sql/sql_datatypes.asp
            string sqlCreateTable = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE NAME = 'PersistMessage')
                BEGIN
                    CREATE TABLE PersistMessage(
                        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
                        DataType NVARCHAR(MAX),
                        Data NVARCHAR(MAX),
                        Created DATETIME2 NOT NULL DEFAULT GETDATE(),
                        RetryCount INT NOT NULL DEFAULT 0,
                        MessageStatus TINYINT NOT NULL DEFAULT 1,
                        DeliveryType TINYINT NOT NULL DEFAULT 1,
                        Version BIGINT NOT NULL DEFAULT -1,
                    );
                END
            ";

            Database.ExecuteSqlRaw(sqlCreateTable);
        }
    }
}
