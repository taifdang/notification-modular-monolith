using BuildingBlocks.Exception;

namespace Wallet.Wallets.Exceptions;

public class WalletIdNotExistException : DomainException
{
    public WalletIdNotExistException(Guid Id) : base($"WalletId: {Id} not exist in database")
    {
    }
}
