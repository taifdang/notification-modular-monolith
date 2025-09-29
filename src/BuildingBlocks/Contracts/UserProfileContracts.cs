
using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record PreferenceDto(ChannelType Channel, bool IsOptOut);
public record BalanceUpdated(int TopupId, Guid UserId, decimal Amount) : IIntegrationEvent;
public record BalanceUpdateFailed(int TopupId, Guid UserId, decimal Amount, string Reason) : IIntegrationEvent;

