using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Domain.Events;

public class IntegrationEventWrapper<T> : IIntegrationEvent where T : IDomainEvent
{
    public T DomainEvent { get; }

    public IntegrationEventWrapper(T domainEvent)
    {
        DomainEvent = domainEvent;
    }
}
