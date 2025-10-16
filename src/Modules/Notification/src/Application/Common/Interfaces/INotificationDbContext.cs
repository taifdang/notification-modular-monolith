
using BuildingBlocks.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Notification.Application.Common.Interfaces;

public interface INotificationDbContext : IDbContext
{
    public DbSet<Domain.Entities.Notification> Notifications { get; }
    public DbSet<Domain.Entities.Recipient> Recipients { get; }
    public DbSet<Domain.Entities.Template> Templates { get; }
    public DbSet<Domain.Entities.NotificationLog> NotificationLogs { get; }
}
