using UserProfile.UserProfiles.Exceptions;

namespace UserProfile.UserProfiles.ValueObjects;

public class Name
{
    public string Value { get; }
    private Name(string value)
    {
        Value = value;
    }
    public static Name Of(string value)
    {
        if (value == string.Empty)
        {
            throw new InvalidNameExeption();
        }
        return new Name(value);
    }
    public static implicit operator string(Name name)
    {
        return name.Value;
    }
}
