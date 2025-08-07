using Topup.Topups.Exceptions;

namespace Topup.Topups.ValueObjects;
public record TransactionId
{
    public int Value { get; }

    private TransactionId (int value)
    {
        Value = value;
    }

    public static TransactionId Of(int value)
    {
        if(value < 0)
        {
            throw new InvalidTransactionIdException();
        }

        return new TransactionId (value);
    }

    public static implicit operator int (TransactionId transactionId)
    {
        return transactionId.Value;
    }

}
