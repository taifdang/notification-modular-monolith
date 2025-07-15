using Hookpay.Modules.Notifications.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Data.Configurations;

public class OutboxMessageConfiguartion : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessage", "dbo");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(x => x.Status).IsRequired().HasDefaultValue(MessageStatus.Pending);
        builder.Property(x => x.MessageType).IsRequired().HasDefaultValue(MessageType.All);
        builder.Property(x=>x.IsDeleted).IsRequired().HasDefaultValue(false);
    }
}
