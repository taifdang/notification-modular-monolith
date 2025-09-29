using BuildingBlocks.Exception;

namespace Wallet.Transactions.Exceptions;
public class PaymentCodeNotExistException : DomainException
{
    public PaymentCodeNotExistException(string? PaymentCode) : base($"PaymentCode: {PaymentCode} not exist in database.")
    {
    }
}
