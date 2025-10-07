using BuildingBlocks.EFCore;
using BuildingBlocks.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Profile.Data;

public class ProfileDbContext : AppDbContextBase
{
    public ProfileDbContext(DbContextOptions<ProfileDbContext> options, ILogger<AppDbContextBase>? logger = null, ICurrentUserProvider? currentUserProvider = null) : base(options, logger, currentUserProvider)
    {
    }
}
