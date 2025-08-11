using BuildingBlocks.EFCore;
using BuildingBlocks.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UserProfile.Data;

using UserProfile.UserPreferences.Model;
using UserProfile.UserProfiles.Model;

public class UserProfileDbContext : AppDbContextBase
{
    public UserProfileDbContext(DbContextOptions<UserProfileDbContext> options,
        ILogger<AppDbContextBase>? logger = null, ICurrentUserProvider? currentUserProvider = null) 
        : base(options, logger, currentUserProvider)
    {
    }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserPreference> UserPreferences => Set<UserPreference>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(UserProfileRoot).Assembly);
    }
}
