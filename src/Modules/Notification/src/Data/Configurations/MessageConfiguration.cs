using Microsoft.EntityFrameworkCore;

namespace Notification.Data.Configurations;

using BuildingBlocks.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Notifications.Enums;
using Notification.Notifications.Model;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable(nameof(Message));

        builder.HasKey(x => x.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(x => x.Channel)
        .HasDefaultValue(ChannelType.None)
        .HasConversion(
            x => x.ToString(),
            x => (ChannelType)Enum.Parse(typeof(ChannelType), x));

        builder.Property(r => r.Body).IsRequired();

        builder.Property(x => x.Status)
       .HasDefaultValue(MessageStatus.None)
       .HasConversion(
           x => x.ToString(),
           x => (MessageStatus)Enum.Parse(typeof(MessageStatus), x));

        builder
            .HasOne<Notification>()
            .WithMany()
            .HasForeignKey(x => x.NotificationId)
            .IsRequired();
    }
}
