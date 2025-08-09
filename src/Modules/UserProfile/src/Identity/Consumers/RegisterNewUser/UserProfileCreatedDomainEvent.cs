using BuildingBlocks.Core.Event;

namespace UserProfile.Identity.Consumers.RegisterNewUser;

public record UserProfileCreatedDomainEvent(Guid Id, Guid UserId, string Name, bool IsDeleted = false) : IDomainEvent;
