using User.Profiles.Exceptions;

namespace User.Profiles.ValueObjects;

public record UserName
{
    public string Value { get; }
    private UserName(string value)
    {
        Value = value;
    }
    public static UserName Of(string value)
    {
        if (value == string.Empty)
        {
            throw new InvalidUserNameException();
        }
        return new UserName(value);
    }
    public static implicit operator string(UserName userName)
    {
        return userName.Value;
    }
}
