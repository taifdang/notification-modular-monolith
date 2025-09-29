


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wallet.Data.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallets.Models.Wallet>
{
    public void Configure(EntityTypeBuilder<Wallets.Models.Wallet> builder)
    {
        throw new NotImplementedException();
    }
}
