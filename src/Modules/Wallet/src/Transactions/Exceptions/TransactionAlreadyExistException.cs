using BuildingBlocks.Exception;
using System.Net;

namespace Wallet.Transactions.Exceptions;

public class TransactionAlreadyExistException : DomainException
{
    public TransactionAlreadyExistException(int? code = default)
        : base("Transaction already exist!", HttpStatusCode.Conflict, code)
    {
    }
}
