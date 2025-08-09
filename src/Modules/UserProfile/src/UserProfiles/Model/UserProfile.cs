using BuildingBlocks.Core.Model;
using UserProfile.Identity.Consumers.RegisterNewUser;
using UserProfile.UserProfiles.Enums;
using UserProfile.UserProfiles.Features.CompletingRegisterUserProfile;
using UserProfile.UserProfiles.ValueObjects;

namespace UserProfile.UserProfiles.Model;

public record UserProfile : Aggregate<UserProfileId>
{
    public UserId UserId { get; private set; } = default!;
    public Name Name { get; private set; } = default!;
    public GenderType GenderType { get; private set; }
    public Age? Age { get; private set; }

    public UserProfile Create(UserProfileId userProfileId,UserId userId, Name name, bool isDeleted = false)
    {
        var userProfile = new UserProfile
        {
            Id = userProfileId,
            UserId = userId,
            Name = name,
            IsDeleted = isDeleted
        };

        var @event = new UserProfileCreatedDomainEvent(userProfile.Id,userProfile.UserId,userProfile.Name,userProfile.IsDeleted);

        userProfile.AddDomainEvent(@event);

        return userProfile;
    }
    public void CompleteRegistrationUserProfile(UserProfileId userProfileId, UserId userId, Name name,
        GenderType genderType, Age age, bool isDeleted = false)
    {
        this.Id = userProfileId;
        this.UserId = userId;
        this.Name = name;
        this.GenderType = genderType;
        this.Age = age;
        this.IsDeleted = isDeleted;

        var @event = new UserProfileRegistrationCompletedDomainEvent(this.Id, this.UserId, this.Name,
            this.GenderType, this.Age, this.IsDeleted);

        this.AddDomainEvent(@event);
    }
}
