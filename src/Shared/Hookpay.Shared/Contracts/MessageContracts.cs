using Hookpay.Shared.Domain.Events;

namespace Hookpay.Shared.Contracts;

public record MessageCreated(Guid correlationId, string eventType, string payload) : IIntegrationEvent;
public record MessageAllContracts(Guid correlationId, int? userId, string title, string body) : IIntegrationEvent;
public record MessagePersonalContracts(Guid correlationId, int? userId, string title, string body) : IIntegrationEvent;
