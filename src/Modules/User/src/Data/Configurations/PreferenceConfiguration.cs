namespace User.Data.Configurations;

using BuildingBlocks.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Preferences.Model;
using User.Preferences.ValueObjects;
public class PreferenceConfiguration : IEntityTypeConfiguration<Preference>
{
    public void Configure(EntityTypeBuilder<Preference> builder)
    {
        builder.ToTable(nameof(Preference));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever()
            .HasConversion<Guid>(n => n.Value, id => PreferenceId.Of(id));

        builder.Property(x => x.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.UserId,
            a =>
            {
                a.Property(x => x.Value)
                    .HasColumnName(nameof(Preference.UserId))
                    .IsRequired();
                //userId is not unique because user can have multiple preferences for different channels
            }
        );

        builder.Property(x => x.Channel)
            .HasDefaultValue(ChannelType.InApp)
            .HasConversion(
                x => x.ToString(),
                x => (ChannelType)Enum.Parse(typeof(ChannelType), x));
    }
}
