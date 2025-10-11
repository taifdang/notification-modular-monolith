using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Signalr;

public class ConnectionDbContext : DbContext, IConnectionDbContext
{
    private readonly ILogger<ConnectionDbContext> _logger;

    public ConnectionDbContext(
        DbContextOptions<ConnectionDbContext> options,
        ILogger<ConnectionDbContext> logger) : base(options)
    {
        _logger = logger;
    }

    public DbSet<Connection> Connection => Set<Connection>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        //ref: https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations#resolving-concurrency-conflicts
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

                if (databaseValues is null)
                    _logger.LogWarning("The record no longer exists in the database, The record has been deleted by another user.");

                // Refresh original values to bypass next concurrency check
                entry.CurrentValues.SetValues(databaseValues);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
    public Task ExecuteTransactionAsync(CancellationToken cancellationToken = default)
    {
        var stratery = Database.CreateExecutionStrategy();

        return stratery.ExecuteAsync(async () =>
        {
            await using var transaction =
                await Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
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

    public void CreateTableIfNotExist()
    {
        //ref: https://www.w3schools.com/sql/sql_datatypes.asp
        string sqlCreateTable = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE NAME = 'Connection')
                BEGIN
                    CREATE TABLE Connection(
                        Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
                        UserId UNIQUEIDENTIFIER NOT NULL,
                        ConnectionId NVARCHAR(MAX) NOT NULL,
                        IsConnected BIT NOT NULL DEFAULT 1,
                        ConnectionAt DATETIME NOT NULL DEFAULT GETDATE(),
                        DisconnectionAt DATETIME,
                        TokenExpiry BIGINT
                    );
                END
            ";

        Database.ExecuteSqlRaw(sqlCreateTable);
    }
}
