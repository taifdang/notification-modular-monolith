using BuildingBlocks.EFCore;
using BuildingBlocks.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Topup.Data;
public class TopupDbContext : AppDbContextBase
{
    public TopupDbContext(
        DbContextOptions<TopupDbContext> options,
        ILogger<AppDbContextBase>? logger = null,
        ICurrentUserProvider? currentUserProvider = null) 
        : base(options, logger, currentUserProvider)
    {
    }
    public DbSet<Topups.Models.Topup> Topups => Set<Topups.Models.Topup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
}
