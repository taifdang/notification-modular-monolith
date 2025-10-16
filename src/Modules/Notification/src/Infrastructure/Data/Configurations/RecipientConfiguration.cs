using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notification.Infrastructure.Data.Configurations;

public class RecipientConfiguration : IEntityTypeConfiguration<Domain.Entities.Recipient>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Recipient> builder)
    {
        builder.ToTable(nameof(Domain.Entities.Recipient));

        builder.HasKey(x => x.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(r => r.UserId).IsRequired();

        builder
            .HasOne<Domain.Entities.Notification>()
            .WithMany()
            .HasForeignKey(x => x.NotificationId)
            .IsRequired();

        builder.Property(x => x.Channel)
            .HasDefaultValue(BuildingBlocks.Contracts.ChannelType.InApp)
            .HasConversion(
                x => x.ToString(),
                x => (BuildingBlocks.Contracts.ChannelType)Enum.Parse(typeof(BuildingBlocks.Contracts.ChannelType), x));
    }
}
