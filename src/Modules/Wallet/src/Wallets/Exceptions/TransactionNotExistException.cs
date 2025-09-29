using BuildingBlocks.Exception;

namespace Wallet.Wallets.Exceptions;
public class TransactionNotExistException : DomainException
{
    public TransactionNotExistException(Guid Id) : base($"TransactionId: {Id} not exist in database")
    {
    }
}
