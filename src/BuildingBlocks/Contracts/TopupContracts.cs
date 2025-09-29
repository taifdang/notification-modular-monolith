
using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record TopupCreated(Guid CorrelationId ,int Id, string UserName, decimal TransferAmount) : IIntegrationEvent;
