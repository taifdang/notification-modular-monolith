
using BuildingBlocks.Exception;

namespace Wallet.Transactions.Exceptions;

public class InvalidTransactionIdException : DomainException
{
    public InvalidTransactionIdException(Guid transactionId)
        : base($"TransactionId: {transactionId} is invalid")
    {

    }
}
