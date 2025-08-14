using UserProfile.UserPreferences.Exceptions;

namespace UserProfile.UserPreferences.ValueObject;

public record Preference
{
    public string Value { get; }
    private Preference(string value)
    {
        Value = value;
    }
    public static Preference Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidPreferenceException();
        }
        return new Preference(value);
    }
    public static implicit operator string(Preference preference)
    {
        return preference.Value;
    }
}
