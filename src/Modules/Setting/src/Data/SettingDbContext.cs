using BuildingBlocks.EFCore;
using BuildingBlocks.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Setting.Data;
public class SettingDbContext : AppDbContextBase
{
    public SettingDbContext(DbContextOptions<SettingDbContext> options, ILogger<AppDbContextBase>? logger = null, ICurrentUserProvider? currentUserProvider = null) : base(options, logger, currentUserProvider)
    {
    }
    public DbSet<NotificationPreferences.Model.NotificationPreference> NotificationPreferences => Set<NotificationPreferences.Model.NotificationPreference>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SettingDbContext).Assembly);
    }
}
