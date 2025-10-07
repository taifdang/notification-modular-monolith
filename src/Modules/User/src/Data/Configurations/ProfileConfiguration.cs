using Microsoft.EntityFrameworkCore;
using User.Profiles.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Profiles.ValueObjects;

namespace User.Data.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable(nameof(Profile));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever()
            .HasConversion<Guid>(userProfile => userProfile.Value, id => ProfileId.Of(id));

        builder.Property(x => x.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.UserId,
            a =>
            {
                a.Property(x => x.Value)
                    .HasColumnName(nameof(Profile.UserId))
                    .IsRequired();

                a.HasIndex(x => x.Value).IsUnique();
            }
        );
        builder.OwnsOne(
          x => x.UserName,
          a =>
          {
              a.Property(x => x.Value)
                  .HasColumnName(nameof(Profile.UserName))
                  .IsRequired();
          }
      );

        builder.OwnsOne(
           x => x.Name,
           a =>
           {
               a.Property(x => x.Value)
                   .HasColumnName(nameof(Profile.Name))
                   .IsRequired();
           }
       );

        builder.OwnsOne(
          x => x.Email,
          a =>
          {
              a.Property(x => x.Value)
                  .HasColumnName(nameof(Profile.Email))
                  .IsRequired();
          }
      );

        builder.Property(x => x.GenderType)
            .HasDefaultValue(Profiles.Enums.GenderType.Unknown)
            .HasConversion(
                x => x.ToString(),
                x => (Profiles.Enums.GenderType)Enum.Parse(typeof(Profiles.Enums.GenderType), x));

        builder.OwnsOne(
           x => x.Age,
           a =>
           {
               a.Property(x => x.Value)
                   .HasColumnName(nameof(Profile.Age))
                   .IsRequired();
           }
       );
    }
}
