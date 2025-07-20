using Hookpay.Modules.Users.Core.Users.Enums;
using Hookpay.Modules.Users.Core.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<Users.Models.Users>
{
    public void Configure(EntityTypeBuilder<Users.Models.Users> builder)
    {
        builder.ToTable("Users", "dbo");

        builder.HasKey(x => x.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(x => x.Email)
            .IsUnique();
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(108)
            .HasDefaultValue("");

        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(25)
            .HasDefaultValue("");
            

        builder.Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(x => x.Phone)           
            .HasMaxLength(13);

        builder.Property(x => x.Balance)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0.0);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(UserStatus.Active);

        builder.Property(x => x.Status).IsRequired().HasDefaultValue(UserStatus.Active);

        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

        builder.HasOne(x => x.UserSetting)
            .WithOne(x => x.Users)
            .HasForeignKey<UserSetting>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
    }
}
