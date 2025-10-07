
using User.Preferences.Exceptions;

namespace User.Preferences.ValueObjects;
public record PreferenceId
{
    public Guid Value { get; }
    private PreferenceId(Guid value)
    {
        Value = value;
    }
    public static PreferenceId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidPreferenceIdException(value);
        }
        return new PreferenceId(value);
    }
    public static implicit operator Guid(PreferenceId userPreferenceId)
    {
        return userPreferenceId.Value;
    }
}
