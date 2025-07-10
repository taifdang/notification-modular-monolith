using Hookpay.Modules.Users.Core.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.user_id);
        builder.Property(u => u.user_id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(x => x.user_email)
            .IsUnique();
        builder.Property(x => x.user_email)
            .IsRequired()
            .HasMaxLength(108)
            .HasDefaultValue(default);

        builder.Property(x => x.user_name)
            .IsRequired()
            .HasMaxLength(25)
            .HasDefaultValue(default);
            

        builder.Property(x => x.user_password)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(x => x.user_phone)           
            .HasMaxLength(13);

        builder.Property(x => x.user_balance)
            .HasColumnType("decimail(18,2")
            .HasDefaultValue(default);

        builder.Property(x => x.is_block)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasOne(x => x.settings)
            .WithOne(x => x.users)
            .HasForeignKey<UserSetting>(x => x.set_user_id);
    }
}
