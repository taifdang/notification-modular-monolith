
using BuildingBlocks.EFCore;
using BuildingBlocks.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Wallet.Data;

public class WalletDbContext : AppDbContextBase
{
    public WalletDbContext(DbContextOptions<WalletDbContext> options, 
        ILogger<AppDbContextBase>? logger = null, ICurrentUserProvider? currentUserProvider = null) 
        : base(options, logger, currentUserProvider)
    {
    }

    public DbSet<Wallets.Models.Wallet> Wallets => Set<Wallets.Models.Wallet>();
    public DbSet<Transactions.Models.Transaction> Transactions => Set<Transactions.Models.Transaction>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);   
    }
}
