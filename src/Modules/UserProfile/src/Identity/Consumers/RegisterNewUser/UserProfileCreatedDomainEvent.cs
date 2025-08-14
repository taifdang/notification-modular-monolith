using BuildingBlocks.Core.Event;

namespace UserProfile.Identity.Consumers.RegisterNewUser;

public record UserProfileCreatedDomainEvent(Guid Id, Guid UserId,string UserName, string Name, bool IsDeleted = false) : IDomainEvent;
