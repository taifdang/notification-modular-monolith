using BuildingBlocks.Core.Event;

namespace BuildingBlocks.Core;

public record IntegrationEventWrapper<TDomainEvent>(TDomainEvent DomainEvent) : IIntegrationEvent
    where TDomainEvent : IDomainEvent;