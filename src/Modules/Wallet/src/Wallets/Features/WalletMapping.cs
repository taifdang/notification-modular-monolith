using Mapster;

namespace Wallet.Wallets.Features;

public class WalletMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Models.Wallet, Dtos.WalletDto>()
           .ConstructUsing(x => new Dtos.WalletDto(x.Id.Value, x.UserId, x.PaymentCode, x.Balance.Value));
    }
}

