using BuildingBlocks.Exception;
using System.Net;

namespace Topup.Topups.Exceptions;
public class InvalidTransactionIdException : DomainException
{
    public InvalidTransactionIdException() 
        : base("TransactionId cannot be negative.")
    {
    }
}
