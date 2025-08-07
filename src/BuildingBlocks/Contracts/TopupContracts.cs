
using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record TopupCreated(int id) : IIntegrationEvent;
