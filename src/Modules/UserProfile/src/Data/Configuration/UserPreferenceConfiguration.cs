using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfile.UserPreferences.ValueObject;

namespace UserProfile.Data.Configuration;

//ref: https://learn.microsoft.com/en-us/ef/core/modeling/
public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreferences.Model.UserPreference>
{
    public void Configure(EntityTypeBuilder<UserPreferences.Model.UserPreference> builder)
    {
        builder.ToTable(nameof(UserPreferences.Model.UserPreference));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever()
            .HasConversion<Guid>(n => n.Value, id => UserPreferenceId.Of(id));

        builder.Property(x => x.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.UserId,
            a =>
            {
                a.Property(x => x.Value)
                    .HasColumnName(nameof(UserPreferences.Model.UserPreference.UserId))                     
                    .IsRequired();

                a.HasIndex(x => x.Value).IsUnique();
            }
        );
      
        builder.OwnsOne(
           x => x.Preference,
           a =>
           {
               a.Property(x => x.Value)
                   .HasColumnName(nameof(UserPreferences.Model.UserPreference.Preference))
                   .IsRequired();
           }
       );    
    }
}
