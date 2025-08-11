using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfile.NotificationSettings.Model;
using UserProfile.NotificationSettings.ValueObject;

namespace UserProfile.Data.Configuration;

//ref: https://learn.microsoft.com/en-us/ef/core/modeling/
public class NotificationSettingConfiguration : IEntityTypeConfiguration<NotificationSetting>
{
    public void Configure(EntityTypeBuilder<NotificationSetting> builder)
    {
        builder.ToTable(nameof(NotificationSetting));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever()
            .HasConversion<Guid>(n => n.Value, id => NotificationSettingId.Of(id));

        builder.Property(x => x.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.UserId,
            a =>
            {
                a.Property(x => x.Value)
                    .HasColumnName(nameof(UserId))                     
                    .IsRequired();

                a.HasIndex(x => x.Value).IsUnique();
            }
        );
      
        builder.OwnsOne(
           x => x.Preference,
           a =>
           {
               a.Property(x => x.Value)
                   .HasColumnName(nameof(Preference))
                   .IsRequired();
           }
       );    
    }
}
