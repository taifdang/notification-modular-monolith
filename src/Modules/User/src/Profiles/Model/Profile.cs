using BuildingBlocks.Core.Model;
using User.Profiles.Enums;
using User.Profiles.ValueObjects;

namespace User.Profiles.Model;
public record Profile : Aggregate<ProfileId>
{
    public UserId UserId { get; private set; } = default!;
    public UserName UserName { get; private set; } = default!;
    public Name Name { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public GenderType GenderType { get; private set; }
    public Age? Age { get; private set; }

    public static Profile Create(ProfileId userProfileId, UserId userId, UserName userName, Name name, Email email, bool isDeleted = false)
    {
        var userProfile = new Profile
        {
            Id = userProfileId,
            UserId = userId,
            UserName = userName,
            Name = name,
            Email = email,
            IsDeleted = isDeleted
        };

        return userProfile;
    }

    public void Update(ProfileId userProfileId, UserId userId, UserName userName, Name name,
       Email email, GenderType genderType, Age age, bool isDeleted = false)
    {
        this.Id = userProfileId;
        this.UserId = userId;
        this.UserName = userName;
        this.Name = name;
        this.Email = email;
        this.GenderType = genderType;
        this.Age = age;
        this.IsDeleted = isDeleted;
    }
}
