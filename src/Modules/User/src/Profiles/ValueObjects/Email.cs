using User.Profiles.Exceptions;

namespace User.Profiles.ValueObjects;
public record Email
{
    public string Value { get; }
    private Email(string value)
    {
        Value = value;
    }
    public static Email Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidEmailException();
        }
        return new Email(value);
    }
    public static implicit operator string(Email email)
    {
        return email.Value;
    }
}
