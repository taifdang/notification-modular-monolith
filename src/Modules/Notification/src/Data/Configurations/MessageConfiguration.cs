using Microsoft.EntityFrameworkCore;

namespace Notification.Data.Configurations;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Messages.Model;
using Notification.Notifications.Model;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable(nameof(Message));

        builder.HasKey(x => x.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(r => r.Key).IsRequired();
        builder.Property(r => r.Value).IsRequired();

        builder
            .HasOne<Notification>()
            .WithMany()
            .HasForeignKey(x => x.NotificationId)
            .IsRequired();
    }
}
