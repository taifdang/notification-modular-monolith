using BuildingBlocks.Exception;

namespace Topup.Topups.Exceptions;
public class InvalidTransferAmountException : DomainException
{
    public InvalidTransferAmountException() 
        : base("TransferAmount cannot be negative.")
    {
    }
}
