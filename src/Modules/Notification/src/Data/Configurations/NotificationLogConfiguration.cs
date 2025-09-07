using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.NotificationLogs.Model;

namespace Notification.Data.Configurations;

public class NotificationLogConfiguration : IEntityTypeConfiguration<NotificationLog>
{
    public void Configure(EntityTypeBuilder<NotificationLog> builder)
    {
        builder.ToTable(nameof(NotificationLog));

        builder.HasKey(x => x.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.HasOne<Notifications.Model.Notification>()
            .WithMany()
            .HasForeignKey(x => x.NotificationId)
            .IsRequired();

        builder.Property(r => r.Channel)
            .HasDefaultValue(BuildingBlocks.Contracts.ChannelType.InApp)
            .HasConversion(
                x => x.ToString(),
                x => (BuildingBlocks.Contracts.ChannelType)Enum.Parse(typeof(BuildingBlocks.Contracts.ChannelType), x));          

        builder.Property(r => r.Status)
            .HasDefaultValue(NotificationLogs.Enums.Status.Pending)
            .HasConversion(
                x => x.ToString(),
                x => (NotificationLogs.Enums.Status)Enum.Parse(typeof(NotificationLogs.Enums.Status), x));
        
        builder.Property(r => r.RetryCount).HasDefaultValue(0).IsRequired();
    }
}
