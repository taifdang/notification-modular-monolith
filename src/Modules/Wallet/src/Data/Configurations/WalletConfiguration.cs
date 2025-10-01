using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Wallets.ValueObjects;

namespace Wallet.Data.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallets.Models.Wallet>
{
    public void Configure(EntityTypeBuilder<Wallets.Models.Wallet> builder)
    {
        builder.ToTable(nameof(Wallets.Models.Wallet));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever()
            .HasConversion<Guid>(n => n.Value, id => WalletId.Of(id));

        builder.Property(w => w.Version).IsConcurrencyToken();

        builder.Property(w => w.PaymentCode).IsRequired();

        builder.HasIndex(x => x.UserId).IsUnique();

        builder.OwnsOne(
           x => x.Balance,
           a =>
           {
               a.Property(x => x.Value)
                   .HasColumnName(nameof(Balance))
                   .HasDefaultValue(0)
                   .HasPrecision(18, 2)
                   .IsRequired();
           }
       );

    }
}
