using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Contracts;

public record WalletCreated(Guid Id) : IIntegrationEvent;
public record WalletUpdated(Guid Id) : IIntegrationEvent;
public record TransactionCreated(Guid Id) : IIntegrationEvent;
// state machine
public record BalanceUpdatedEvent(Guid TransactionId, Guid UserId) : IIntegrationEvent;
public record BalanceUpdateFailedEvent(Guid TransactionId, string ErrorMessage) : IIntegrationEvent;
public record TopupConfirmedEvent(Guid TransactionId, decimal Amount) : IIntegrationEvent;
public record TopupFailedEvent(Guid TransactionId, string ErrorMessage) : IIntegrationEvent;

