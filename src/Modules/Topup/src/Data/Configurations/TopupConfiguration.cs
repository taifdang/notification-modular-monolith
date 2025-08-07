using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Topup.Data.Configurations;

using Topups.ValueObjects;
using Topups.Models;

public class TopupConfiguration : IEntityTypeConfiguration<Topups.Models.Topup>
{
    public void Configure(EntityTypeBuilder<Topups.Models.Topup> builder)
    {
        builder.ToTable(nameof(Topup));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever()
            .HasConversion<Guid>(topup => topup.Value, id => TopupId.Of(id));

        builder.Property(x => x.Version).IsConcurrencyToken();

        builder.OwnsOne(
            x => x.TransactionId,
            a =>
            {
                a.Property(x => x.Value)
                    .HasColumnName(nameof(TransactionId))
                    .IsRequired();  
            }
        );

        builder.OwnsOne(
            x => x.TransferAmount,
            a =>
            {
                a.Property(x => x.Value)
                    .HasColumnName(nameof(TransferAmount))
                    .HasPrecision(18,2)
                    .HasMaxLength(10)
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.CreateByName,
            a =>
            {
                a.Property(x => x.Value)
                    .HasColumnName(nameof(CreateByName))
                    .HasMaxLength(50)
                    .IsRequired();
            }
        );

        builder.Property(x => x.Status)
            .HasDefaultValue(Topups.Enums.TopupStatus.Unknown)
            .HasConversion(
                x => x.ToString(),
                x => (Topups.Enums.TopupStatus)Enum.Parse(typeof(Topups.Enums.TopupStatus), x));
            

    }
}
