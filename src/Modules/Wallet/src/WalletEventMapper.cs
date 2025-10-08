using BuildingBlocks.Core;
using BuildingBlocks.Core.Event;
using Wallet.Transactions.Features.CreatingTransaction;
using Wallet.Wallets.Features.UpdatingWalletBalance;

namespace Wallet;

public sealed class WalletEventMapper : IEventMapper
{
    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            _ => null
        };
    }

    public IInternalCommand? MapToInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {
            TransactionCreatedDomainEvent e => new UpdateWalletBalanceInternalCommand(e.Id, e.WalletId, e.Amount)
        };
    }
}
