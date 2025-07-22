
using Hookpay.Modules.Topups.Core.Topups.Features;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Core;
using Hookpay.Shared.Domain.Events;

namespace Hookpay.Modules.Topups.Core;

public class TopupEventMapper : IEventMapper
{
    //ref: https://www.ledjonbehluli.com/posts/domain_to_integration_event/
    public IIntegrationEvent? MapIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            TopupCreatedDomainEvent e => new TopupCreated(e.transId, e.creator, e.tranferAmount),
            _ => null
        };
    }

    public IInternalCommand? MapInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {           
            _ => null
        };
    }
}
