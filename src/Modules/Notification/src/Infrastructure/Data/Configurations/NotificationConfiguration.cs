using BuildingBlocks.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notification.Infrastructure.Data.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Domain.Entities.Notification>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Notification> builder)
    {
        builder.ToTable(nameof(Domain.Entities.Notification));

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
    }
}
