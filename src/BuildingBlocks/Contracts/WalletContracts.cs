using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record WalletCreated(Guid Id) : IIntegrationEvent;
public record WalletUpdated(Guid Id) : IIntegrationEvent;
public record TransactionCreated(Guid Id) : IIntegrationEvent;
// state machine
public record BalanceUpdated(Guid TransactionId, Guid UserId) : IIntegrationEvent;
public record TopupConfirmed(Guid TransactionId, decimal Amount) : IIntegrationEvent;
public record TopupFailed(Guid TransactionId, string Reason) : IIntegrationEvent;

