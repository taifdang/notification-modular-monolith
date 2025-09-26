using BuildingBlocks.Core.Event;

namespace UserProfile.Topups.Consumers.CreatingTopup;
public record TopupCreatedDomainEvent(int TopupId, string UserName, decimal TransferAmount) : IDomainEvent;