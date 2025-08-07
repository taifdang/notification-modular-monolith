using Topup.Topups.Exceptions;

namespace Topup.Topups.ValueObjects;
public record TransferAmount
{
    public decimal Value { get; }
    public TransferAmount(decimal value)
    {
        Value = value;
    }
    public static TransferAmount Of(decimal value)
    {
        if(value < 0)
        {
            throw new InvalidTransferAmountException();
        }

        return new TransferAmount(value);
    }
    public static implicit operator decimal(TransferAmount value) 
    {
        return value.Value;
    }
}
