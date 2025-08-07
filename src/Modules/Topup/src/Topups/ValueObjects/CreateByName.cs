using Topup.Topups.Exceptions;

namespace Topup.Topups.ValueObjects;

public class CreateByName
{
    public string Value { get; }

    private CreateByName(string value)
    {
        Value = value;
    }
    public static CreateByName Of(string value)
    {
        if (value == string.Empty)
        {
            throw new InvalidNameException();
        }

        return new CreateByName(value);
    }

    public static implicit operator string(CreateByName createByName)
    {
        return createByName.Value;
    }
}
