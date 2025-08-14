using UserProfile.UserProfiles.Exceptions;

namespace UserProfile.UserProfiles.ValueObjects;

public record UserId
{
    public Guid Value { get; }
    private UserId(Guid value)
    {
        Value = value;
    }
    public static UserId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidUserIdException(value);
        }
        return new UserId(value);
    }
    public static implicit operator Guid(UserId userId)
    {
        return userId.Value;
    }
}
