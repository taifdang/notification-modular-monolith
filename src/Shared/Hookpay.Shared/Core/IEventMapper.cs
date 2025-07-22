
using Hookpay.Shared.Domain.Events;

namespace Hookpay.Shared.Core;

public interface IEventMapper
{
    IIntegrationEvent? MapIntegrationEvent(IDomainEvent @event);
    IInternalCommand? MapInternalCommand(IDomainEvent @event);
}
