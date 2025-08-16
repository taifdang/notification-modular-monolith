using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Recipients.Model;

namespace Notification.Data.Configurations;

public class RecipientConfiguration : IEntityTypeConfiguration<Recipient>
{
    public void Configure(EntityTypeBuilder<Recipient> builder)
    {
        builder.ToTable(nameof(Recipient));

        builder.HasKey(x => x.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder
            .HasOne<Notifications.Model.Notification>()
            .WithMany()
            .HasForeignKey(x => x.NotificationId)
            .IsRequired();
    }
}
