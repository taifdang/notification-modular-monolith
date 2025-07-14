using Hookpay.Modules.Users.Core.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Data.Configuration;

public class UserSettingConfiguration : IEntityTypeConfiguration<UserSetting>
{
    public void Configure(EntityTypeBuilder<UserSetting> builder)
    {
        builder.ToTable("UserSetting", "dbo");

        builder.HasKey(x => x.set_id);
        builder.Property(u => u.set_id)
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.set_user_id)
            .IsRequired();
       
        builder.Property(x => x.disable_notification)
            .IsRequired()
            .HasDefaultValue(false);
      
    }
}
