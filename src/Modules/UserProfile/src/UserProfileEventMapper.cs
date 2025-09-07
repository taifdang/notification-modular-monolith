using BuildingBlocks.Core;
using BuildingBlocks.Core.Event;
using UserProfile.Identity.Consumers.RegisterNewUser;
using UserProfile.UserPreferences.Features.CompletingUserPreference;

namespace UserProfile;

public sealed class UserProfileEventMapper : IEventMapper
{
    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            //UserProfileRegistrationCompletedDomainEvent e => new UserProfileRegistrationCompleted(e.Id),
            //UserProfileCreatedDomainEvent e => new UserProfileCreated(e.Id),
            _ => null
        };
    }

    public IInternalCommand? MapToInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {          
            UserProfileCreatedDomainEvent e => new CompletedUserPreferenceMonoCommand(e.Id, e.UserId),
            _ => null
        };
    }
}
