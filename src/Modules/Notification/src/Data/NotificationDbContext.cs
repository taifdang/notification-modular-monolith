using BuildingBlocks.EFCore;
using BuildingBlocks.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Templates.Model;
using Notification.NotificationLogs.Model;

namespace Notification.Data;
public class NotificationDbContext : AppDbContextBase
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options, ILogger<AppDbContextBase>? logger = null, 
        ICurrentUserProvider? currentUserProvider = null) : base(options, logger, currentUserProvider)
    {
    }

    public DbSet<Notifications.Model.Notification> Notifications => Set<Notifications.Model.Notification>();    
    public DbSet<Recipents.Model.Recipient> Recipients => Set<Recipents.Model.Recipient>();
    public DbSet<Template> Templates => Set<Template>();
    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();

    //public DbSet<NotificationDelivery> NotificationDeliveries => Set<NotificationDelivery>();
    //public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(NotificationRoot).Assembly);

    }
}
