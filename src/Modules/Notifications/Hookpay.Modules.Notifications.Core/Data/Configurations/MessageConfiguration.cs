using Hookpay.Modules.Notifications.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Data.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Message", "dbo");

        builder.HasKey(x=>x.mess_id);
        builder.Property(x => x.mess_id).IsRequired().ValueGeneratedOnAdd();

        builder.HasIndex(x => x.mess_correlationId).IsUnique();

        builder.Property(x => x.mess_processed).IsRequired().HasDefaultValue(false);
    }
}
