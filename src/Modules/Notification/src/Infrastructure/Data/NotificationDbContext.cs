using BuildingBlocks.EFCore;
using BuildingBlocks.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Application.Common.Interfaces;

namespace Notification.Infrastructure.Data;

public class NotificationDbContext : AppDbContextBase, INotificationDbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options, ILogger<AppDbContextBase>? logger = null, ICurrentUserProvider? currentUserProvider = null) : base(options, logger, currentUserProvider)
    {
    }
    public DbSet<Domain.Entities.Notification> Notifications => Set<Domain.Entities.Notification>();
    public DbSet<Domain.Entities.Recipient> Recipients => Set<Domain.Entities.Recipient>();
    public DbSet<Domain.Entities.Template> Templates => Set<Domain.Entities.Template>();
    public DbSet<Domain.Entities.NotificationLog> NotificationLogs => Set<Domain.Entities.NotificationLog>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(NotificationRoot).Assembly);

    }
}
