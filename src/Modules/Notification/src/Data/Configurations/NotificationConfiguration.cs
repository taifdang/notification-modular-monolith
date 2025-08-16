using BuildingBlocks.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Notifications.Enums;

namespace Notification.Data.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notifications.Model.Notification>
{
    public void Configure(EntityTypeBuilder<Notifications.Model.Notification> builder)
    {
        builder.ToTable(nameof(Notifications.Model.Notification));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.NotificationType)
           .HasDefaultValue(NotificationType.UnKnown)
           .HasConversion(
               x => x.ToString(),
               x => (NotificationType)Enum.Parse(typeof(NotificationType), x));

        builder.Property(x => x.Priority)
           .HasDefaultValue(NotificationPriority.Low)
           .HasConversion(
               x => x.ToString(),
               x => (NotificationPriority)Enum.Parse(typeof(NotificationPriority), x));

        builder.Property(x => x.Status)
           .HasDefaultValue(NotificationStatus.None)
           .HasConversion(
               x => x.ToString(),
               x => (NotificationStatus)Enum.Parse(typeof(NotificationStatus), x));
    }
}
