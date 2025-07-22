
using MassTransit;

namespace Hookpay.Shared.Domain.Events;

[ExcludeFromTopology]
public interface IIntegrationEvent:IEvent
{
}
