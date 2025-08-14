using UserProfile.UserProfiles.Exceptions;

namespace UserProfile.UserProfiles.ValueObjects;

public record Balance
{
    public decimal Value { get; }
    private Balance(decimal value)
    {
        Value = value;
    }
    public static Balance Of(decimal value)
    {
        if (value < 0)
        {
            throw new InvalidBalanceException();
        }
        return new Balance(value);
    }
    public static implicit operator decimal(Balance balance)
    {
        return balance.Value;
    }
}
