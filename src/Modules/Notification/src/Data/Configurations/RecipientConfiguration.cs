using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notification.Data.Configurations;

public class RecipientConfiguration : IEntityTypeConfiguration<Recipents.Model.Recipient>
{
    public void Configure(EntityTypeBuilder<Recipents.Model.Recipient> builder)
    {
        builder.ToTable(nameof(Recipents.Model.Recipient));

        builder.HasKey(x => x.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(r => r.UserId).IsRequired();   

        builder
            .HasOne<Notifications.Model.Notification>()
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
