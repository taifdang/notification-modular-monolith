using UserProfile.UserPreferences.Exceptions;

namespace UserProfile.UserPreferences.ValueObject;

public class Preference
{
    public string Value { get; }
    private Preference(string value)
    {
        Value = value;
    }
    public static Preference Of(string value)
    {
        if (value == string.Empty)
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
