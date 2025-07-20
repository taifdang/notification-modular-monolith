using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Models;
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

        builder.HasKey(x=>x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();

        builder.HasIndex(x => x.CorrelationId).IsUnique();

        builder.Property(x => x.Priority).IsRequired().HasDefaultValue(MessagePriority.Low);
        builder.Property(x => x.IsProcessed).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.MessageType).IsRequired().HasDefaultValue(MessageType.All);
        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);
    }
}
