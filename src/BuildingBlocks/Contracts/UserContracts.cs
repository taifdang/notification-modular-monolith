
using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record PreferenceCreated(Guid Id) : IIntegrationEvent;
public record PreferenceUpdated(Guid Id) : IIntegrationEvent;
public record ProfileCreated(Guid Id) : IIntegrationEvent;
public record ProfileUpdated(Guid Id) : IIntegrationEvent;

public record PreferenceDto(ChannelType Channel, bool IsOptOut);


