using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Event;
using UserProfile.Identity.Consumers.RegisterNewUser;
using UserProfile.UserProfiles.Features.CompletingRegisterUserProfile;

namespace UserProfile;

public class UserProfileEventMapper : IEventMapper
{
    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            UserProfileRegistrationCompletedDomainEvent e => new UserProfileRegistrationCompleted(e.Id),
            UserProfileCreatedDomainEvent e => new UserProfileCreated(e.Id),
            _ => null
        };
    }

    public IInternalCommand? MapToInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {
            UserProfileCreatedDomainEvent e => new CompleteRegisterUserProfileMonoCommand(e.Id, e.UserId, e.Name, UserProfiles.Enums.GenderType.Unknown, 0),
            _ => null
        };
    }
}
