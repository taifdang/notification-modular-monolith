using BuildingBlocks.Constants;
using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Event;
using System.Text.Json;
using UserProfile.Identity.Consumers.RegisterNewUser;
using UserProfile.UserPreferences.Features.CompletingUserPreference;
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
            UserProfileCreatedDomainEvent e => new CompletedUserPreferenceMonoCommand(
                e.Id, e.UserId, JsonSerializer.Serialize(NotificationConstant.PreferencesSeed)),
            _ => null
        };
    }
}
