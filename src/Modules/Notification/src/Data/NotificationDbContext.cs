using BuildingBlocks.EFCore;
using BuildingBlocks.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Messages.Model;
using Notification.NotificationDeliveries.Model;
using Notification.Recipients.Model;
using Notification.Templates.Model;

namespace Notification.Data;
public class NotificationDbContext : AppDbContextBase
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options, ILogger<AppDbContextBase>? logger = null, 
        ICurrentUserProvider? currentUserProvider = null) : base(options, logger, currentUserProvider)
    {
    }

    public DbSet<Notifications.Model.Notification> Notifications => Set<Notifications.Model.Notification>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Recipient> Recipients => Set<Recipient>();
    public DbSet<NotificationDelivery> NotificationDeliveries => Set<NotificationDelivery>();
    // DbSet<Template> Templates => Set<Template>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(NotificationRoot).Assembly);

    }
}
