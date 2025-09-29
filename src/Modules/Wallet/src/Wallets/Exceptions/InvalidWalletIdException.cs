using BuildingBlocks.Exception;

namespace Wallet.Wallets.Exceptions;
public class InvalidWalletIdException : DomainException
{
    public InvalidWalletIdException(Guid walletId) : base($"walletId: '{walletId}' is invalid")
    {
    }
}
