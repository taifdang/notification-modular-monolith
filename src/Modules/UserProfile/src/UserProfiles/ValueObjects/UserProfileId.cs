using UserProfile.UserProfiles.Exceptions;

namespace UserProfile.UserProfiles.ValueObjects;

public record UserProfileId
{
    public Guid Value { get; }
    private UserProfileId(Guid value)
    {
        Value = value;
    }
    public static UserProfileId Of(Guid value)
    {
        if(value == Guid.Empty)
        {
            throw new InvalidUserProfileIdException(value);
        }
        return new UserProfileId(value);
    }
    public static implicit operator Guid(UserProfileId userProfileId)
    {
        return userProfileId.Value;
    }
}
