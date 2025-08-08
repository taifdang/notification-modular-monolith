using BuildingBlocks.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Topup.Data.Seed;

public class TopupDataSeeder(TopupDbContext topupDbContext) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        var pendingMigrations = await topupDbContext.Database.GetPendingMigrationsAsync();
        if (!pendingMigrations.Any())
        {
            await SeedTopups();
        }
    }

    public async Task SeedTopups()
    {
        if(!await EntityFrameworkQueryableExtensions.AnyAsync(topupDbContext.Topups))
        {
            await topupDbContext.AddRangeAsync(InitialData.Topups);
            await topupDbContext.SaveChangesAsync();
        }
    }
}
