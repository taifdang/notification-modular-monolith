using BuildingBlocks.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notification.Infrastructure.Data.Configurations;

public class TemplateNotification : IEntityTypeConfiguration<Domain.Entities.Template>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Template> builder)
    {
        builder.ToTable(nameof(Domain.Entities.Template));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Channel)
           .HasDefaultValue(ChannelType.InApp)
           .HasConversion(
               x => x.ToString(),
               x => (ChannelType)Enum.Parse(typeof(ChannelType), x));

        builder.Property(x => x.NotificationType)
            .HasDefaultValue(NotificationType.UnKnown)
            .HasConversion(
                x => x.ToString(),
                x => (NotificationType)Enum.Parse(typeof(NotificationType), x));

    }
}
