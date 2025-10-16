using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notification.Infrastructure.Data.Configurations;

public class NotificationLogConfiguration : IEntityTypeConfiguration<Domain.Entities.NotificationLog>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.NotificationLog> builder)
    {
        builder.ToTable(nameof(Domain.Entities.NotificationLog));

        builder.HasKey(x => x.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.HasOne<Domain.Entities.Notification>()
            .WithMany()
            .HasForeignKey(x => x.NotificationId)
            .IsRequired();

        builder.Property(r => r.Channel)
            .HasDefaultValue(BuildingBlocks.Contracts.ChannelType.InApp)
            .HasConversion(
                x => x.ToString(),
                x => (BuildingBlocks.Contracts.ChannelType)Enum.Parse(typeof(BuildingBlocks.Contracts.ChannelType), x));

        builder.Property(r => r.Status)
            .HasDefaultValue(Domain.Enums.DeliveryStatus.Pending)
            .HasConversion(
                x => x.ToString(),
                x => (Domain.Enums.DeliveryStatus)Enum.Parse(typeof(Domain.Enums.DeliveryStatus), x));

        builder.Property(r => r.RetryCount).HasDefaultValue(0).IsRequired();
    }
}

