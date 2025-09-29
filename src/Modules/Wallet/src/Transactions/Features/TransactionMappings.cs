using Mapster;
using Wallet.Transactions.Features.CreatingTransaction;

namespace Wallet.Transactions.Features;

public class TransactionMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateTransactionRequestDto, CreateTransaction>()
           .ConstructUsing(x => new CreateTransaction(x.Id,x.AccountNumber,x.Description,x.TransferAmount));
    }
}
