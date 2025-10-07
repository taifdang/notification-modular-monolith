using BuildingBlocks.Core.Event;

namespace User.Identity.Consumers.RegisteringNewUser;
public record UserCreatedDomainEvent(Guid Id, Guid UserId, string UserName, string Name, string Email, bool IsDeleted = false) : IDomainEvent;
