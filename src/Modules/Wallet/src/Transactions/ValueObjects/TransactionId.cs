using Wallet.Transactions.Exceptions;

namespace Wallet.Transactions.ValueObjects;

public record TransactionId
{
    public Guid Value { get; }

    private TransactionId(Guid value)
    {
        Value = value;
    }
    public static TransactionId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidTransactionIdException(value);
        }
        return new TransactionId(value);
    }
    public static implicit operator Guid(TransactionId transactionId)
    {
        return transactionId.Value;
    } 
}
