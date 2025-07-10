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
        builder.HasKey(x => x.topup_id);
        builder.Property(x => x.topup_id).ValueGeneratedOnAdd();

        builder.Property(x => x.topup_trans_id).IsRequired();

        builder.Property(x => x.topup_source).HasColumnType("varchar(50)");

        builder.Property(x => x.topup_creator).HasColumnType("varchar(20)");

        builder.Property(x => x.topup_tranfer_amount).IsRequired().HasColumnType("decimal(18,2)");

        //builder.Property(x => x.topup_created_at).HasDefaultValueSql("GETUTCDATE()");
    }
}
