using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfile.UserProfiles.ValueObjects;

namespace UserProfile.Data.Configuration;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfiles.Model.UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfiles.Model.UserProfile> builder)
    {
        builder.ToTable(nameof(UserProfiles.Model.UserProfile));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever()
            .HasConversion<Guid>(userProfile => userProfile.Value, id => UserProfileId.Of(id));

        builder.OwnsOne(
            x => x.UserId,
            a =>
            {
                a.Property(x => x.Value)
                    .HasColumnName(nameof(UserId))
                    .IsRequired();
            }
        );

        builder.OwnsOne(
           x => x.Name,
           a =>
           {
               a.Property(x => x.Value)
                   .HasColumnName(nameof(Name))
                   .IsRequired();
           }
       );

        builder.Property(x => x.GenderType)
            .HasDefaultValue(UserProfiles.Enums.GenderType.Unknown)
            .HasConversion(
                x => x.ToString(),
                x => (UserProfiles.Enums.GenderType)Enum.Parse(typeof(UserProfiles.Enums.GenderType), x));

        builder.OwnsOne(
           x => x.Age,
           a =>
           {
               a.Property(x => x.Value)
                   .HasColumnName(nameof(Age))
                   .IsRequired();
           }
       );
    }
}
