using BuildingBlocks.Core.Event;

namespace UserProfile.Identity.Consumers.RegisterNewUser;

public record UserProfileCreatedDomainEvent(Guid Id,Guid UserId,string UserName,string Name,string Email,bool IsDeleted = false) : IDomainEvent;
