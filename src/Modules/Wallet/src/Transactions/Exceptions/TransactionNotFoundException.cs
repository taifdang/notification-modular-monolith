using BuildingBlocks.Exception;

namespace Wallet.Transactions.Exceptions;

public class TransactionNotFoundException : DomainException
{
    public TransactionNotFoundException() : base("Transaction not found")
    {
    }
}
