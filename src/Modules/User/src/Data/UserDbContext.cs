using BuildingBlocks.EFCore;
using BuildingBlocks.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Preferences.Model;
using User.Profiles.Model;

namespace User.Data;

public class UserDbContext : AppDbContextBase
{
    public UserDbContext(DbContextOptions<UserDbContext> options, ILogger<AppDbContextBase>? logger = null, ICurrentUserProvider? currentUserProvider = null) : base(options, logger, currentUserProvider)
    {
    }

    public DbSet<Preference> Preferences => Set<Preference>();
    public DbSet<Profile> Profiles => Set<Profile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
    }
}
