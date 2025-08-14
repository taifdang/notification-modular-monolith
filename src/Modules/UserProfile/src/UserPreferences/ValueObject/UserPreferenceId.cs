using UserProfile.UserPreferences.Exceptions;

namespace UserProfile.UserPreferences.ValueObject;

public record UserPreferenceId
{
    public Guid Value { get; }
    private UserPreferenceId(Guid value)
    {
        Value = value;
    }
    public static UserPreferenceId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidUserPreferenceIdException(value);
        }
        return new UserPreferenceId(value);
    }
    public static implicit operator Guid(UserPreferenceId userPreferenceId)
    {
        return userPreferenceId.Value;
    }
}
