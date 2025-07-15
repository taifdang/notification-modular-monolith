using Hookpay.Modules.Topups.Core.Topups.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core.Data.Configurations;

public class TopupConfiguration : IEntityTypeConfiguration<Topup>
{
    public void Configure(EntityTypeBuilder<Topup> builder)
    {
        builder.ToTable("Topup", "dbo");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.TransactionId).IsRequired();
        builder.HasIndex(x => x.TransactionId).IsUnique();

        builder.Property(x => x.Source).HasColumnType("varchar(50)");

        builder.Property(x => x.Creator).HasColumnType("varchar(20)");

        builder.Property(x => x.TransferAmount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

        //builder.Property(x => x.topup_created_at).HasDefaultValueSql("GETUTCDATE()");
    }
}
