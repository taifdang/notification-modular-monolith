using Hookpay.Modules.Users.Core.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Data.Configuration;

public class UserSettingConfiguration : IEntityTypeConfiguration<Users.Models.UserSetting>
{
    public void Configure(EntityTypeBuilder<Users.Models.UserSetting> builder)
    {
        builder.ToTable("UserSetting", "dbo");

        builder.HasKey(x => x.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.UserId)
            .IsRequired();
       
        builder.Property(x => x.AllowNotification)
            .IsRequired()
            .HasDefaultValue(false);
      
    }
}
