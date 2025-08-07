using Topup.Topups.Exceptions;

namespace Topup.Topups.ValueObjects;

public record TopupId
{
    public Guid Value { get; } 

    private TopupId(Guid value)
    {
        Value = value;
    }
    public static TopupId Of(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new InvalidTopupIdException(value);
        }

        return new TopupId(value);
    }

    public static implicit operator Guid(TopupId topupId)
    {
        return topupId.Value;
    }
}
