
using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record TopupCreated(int Id, string UserName, decimal TransferAmount) : IIntegrationEvent;
