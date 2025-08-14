
using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record UserCreated(Guid Id, string UserName ,string Name) : IIntegrationEvent;

