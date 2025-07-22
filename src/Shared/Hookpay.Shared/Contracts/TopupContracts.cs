using Hookpay.Shared.Domain.Events;

namespace Hookpay.Shared.Contracts;

public record TopupCreated(int transId, string username, decimal transferAmount) : IIntegrationEvent;


