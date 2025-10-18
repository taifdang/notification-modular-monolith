using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Wallet.Data;

public class TopupSagaDbContextDesignFactory : IDesignTimeDbContextFactory<TopupSagaDbContext>
{
    public TopupSagaDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<TopupSagaDbContext>();
        builder.UseSqlServer("Server=localhost,1433;Database=notification_db;User Id=sa;Password=Password!;Trust Server Certificate=True");
        return new TopupSagaDbContext(builder.Options);
    }
}
