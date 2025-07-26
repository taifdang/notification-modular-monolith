using MassTransit;

namespace Hookpay.Shared.Core.Events;

[ExcludeFromTopology]
public interface IIntegrationEvent:IEvent
{
}
