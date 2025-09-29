using BuildingBlocks.Exception;

namespace Wallet.Wallets.Exceptions;
public class WalletAlreadyExistException : DomainException
{
    public WalletAlreadyExistException() : base("WalletId is exist")
    {
    }
}
