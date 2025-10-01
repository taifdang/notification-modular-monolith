
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Transactions.Enums;
using Wallet.Transactions.Models;
using Wallet.Transactions.ValueObjects;

namespace Wallet.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable(nameof(Transaction));

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever()
            .HasConversion<Guid>(id => id.Value, value => TransactionId.Of(value));
        builder.Property(t => t.Version).IsConcurrencyToken();

        builder
          .HasOne<Wallets.Models.Wallet>()
          .WithMany()
          .HasForeignKey(x => x.WalletId);

        builder.Property(x => x.Amount)
            .HasDefaultValue(0)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.TransactionStatus)
           .HasDefaultValue(TransactionStatus.Unknown)
           .HasConversion(
               x => x.ToString(),
               x => (TransactionStatus)Enum.Parse(typeof(TransactionStatus), x));

        builder.Property(x => x.TransactionType)
           .HasDefaultValue(TransactionType.None)
           .HasConversion(
               x => x.ToString(),
               x => (TransactionType)Enum.Parse(typeof(TransactionType), x));
    }
}
