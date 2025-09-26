
using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record UserProfileRegistrationCompleted(Guid Id) : IIntegrationEvent;
public record UserProfileCreated(Guid Id) : IIntegrationEvent;
public record UserPreferenceResult(Guid Id, Guid UserId, string Preference);
public record PreferenceDto(ChannelType Channel, bool IsOptOut);


public record BalanceUpdated(int TopupId, Guid UserId, decimal Amount) : IIntegrationEvent;
public record UserBalanceUpdated(Guid CorrelationId, Guid TopupId, Guid UserId, decimal Amount);
