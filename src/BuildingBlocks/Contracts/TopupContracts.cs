
using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record TopupCreated(int id, string username, decimal transferAmount) : IIntegrationEvent;
