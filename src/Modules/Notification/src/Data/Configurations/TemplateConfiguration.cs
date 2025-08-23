using BuildingBlocks.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Templates.Enums;
using Notification.Templates.Model;

namespace Notification.Data.Configurations;

public class TemplateConfiguration : IEntityTypeConfiguration<Template>
{
    public void Configure(EntityTypeBuilder<Template> builder)
    {
        builder.ToTable(nameof(Template));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Language)
           .HasDefaultValue(LanguageType.EN)
           .HasConversion(
               x => x.ToString(),
               x => (LanguageType)Enum.Parse(typeof(LanguageType), x));

        builder.Property(x => x.Channel)
           .HasDefaultValue(ChannelType.None)
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
