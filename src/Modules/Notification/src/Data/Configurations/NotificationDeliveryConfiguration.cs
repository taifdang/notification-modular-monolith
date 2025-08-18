using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.NotificationDeliveries.Model;

namespace Notification.Data.Configurations;

public class NotificationDeliveryConfiguration : IEntityTypeConfiguration<NotificationDelivery>
{
    public void Configure(EntityTypeBuilder<NotificationDelivery> builder)
    {
        builder.ToTable(nameof(NotificationDelivery));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder
            .HasOne<Notifications.Model.Notification>()
            .WithMany()
            .HasForeignKey(x => x.NotificationId)
            .IsRequired();

        builder.Property(x => x.NotificationType)
            .HasDefaultValue(BuildingBlocks.Contracts.NotificationType.UnKnown)
            .HasConversion(
                x => x.ToString(),
                x => (BuildingBlocks.Contracts.NotificationType)Enum.Parse(typeof(BuildingBlocks.Contracts.NotificationType), x));

        builder.Property(x => x.ChannelType)
            .HasDefaultValue(BuildingBlocks.Contracts.ChannelType.None)
            .HasConversion(
                x => x.ToString(),
                x => (BuildingBlocks.Contracts.ChannelType)Enum.Parse(typeof(BuildingBlocks.Contracts.ChannelType), x));

        builder.Property(x => x.Message).IsRequired();


        builder.Property(x => x.DeliveryStatus)
           .HasDefaultValue(NotificationDeliveries.Enums.DeliveryStatus.None)
           .HasConversion(
               x => x.ToString(),
               x => (NotificationDeliveries.Enums.DeliveryStatus)Enum.Parse(typeof(NotificationDeliveries.Enums.DeliveryStatus), x));

        builder.Property(x => x.Priority)
           .HasDefaultValue(BuildingBlocks.Contracts.NotificationPriority.Low)
           .HasConversion(
               x => x.ToString(),
               x => (BuildingBlocks.Contracts.NotificationPriority)Enum.Parse(typeof(BuildingBlocks.Contracts.NotificationPriority), x));
    }
}
