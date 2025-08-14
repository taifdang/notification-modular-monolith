using BuildingBlocks.Core.Model;
using UserProfile.Identity.Consumers.RegisterNewUser;
using UserProfile.UserProfiles.Enums;
using UserProfile.UserProfiles.Features.CompletingRegisterUserProfile;
using UserProfile.UserProfiles.ValueObjects;

namespace UserProfile.UserProfiles.Model;

public record UserProfile : Aggregate<UserProfileId>
{
    public UserId UserId { get; private set; } = default!;
    public UserName UserName { get; private set; } = default!;//option*
    public Name Name { get; private set; } = default!;
    public GenderType GenderType { get; private set; }
    public Age? Age { get; private set; }
    public Balance Balance { get;  set; } = default!;

    public static UserProfile Create(UserProfileId userProfileId,UserId userId, UserName userName, Name name, bool isDeleted = false)
    {
        var userProfile = new UserProfile{Id = userProfileId,UserId = userId,UserName = userName,Name = name,IsDeleted = isDeleted};

        var @event = new UserProfileCreatedDomainEvent(userProfile.Id,userProfile.UserId,userProfile.UserName,userProfile.Name,
            userProfile.IsDeleted);

        userProfile.AddDomainEvent(@event);

        return userProfile;
    }
    public void CompleteRegistrationUserProfile(UserProfileId userProfileId, UserId userId,UserName userName, Name name,
        GenderType genderType, Age age, Balance balance, bool isDeleted = false)
    {
        this.Id = userProfileId;
        this.UserId = userId;
        this.UserName = userName;
        this.Name = name;
        this.GenderType = genderType;
        this.Age = age;
        this.Balance = balance;
        this.IsDeleted = isDeleted;

        var @event = new UserProfileRegistrationCompletedDomainEvent(this.Id, this.UserId,this.UserName, this.Name,
            this.GenderType, this.Age, this.Balance, this.IsDeleted);

        this.AddDomainEvent(@event);
    }
    public void Deposit(Balance balance)
    {
        this.Balance = balance;
    }
}
