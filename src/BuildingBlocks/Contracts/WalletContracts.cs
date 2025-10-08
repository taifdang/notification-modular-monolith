using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record WalletCreated(Guid Id) : IIntegrationEvent;
public record WalletUpdated(Guid Id) : IIntegrationEvent;
public record TransactionCreated(Guid Id) : IIntegrationEvent;
