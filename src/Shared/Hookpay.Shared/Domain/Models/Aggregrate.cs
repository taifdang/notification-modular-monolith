using Hookpay.Shared.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Domain.Models;

public abstract class Aggregate : Aggregrate<int>
{
}
public abstract class Aggregrate<T> : Entity, IAggregate<T>
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public int Version { get; set; } = -1;

    //public T Id { get; protected set; }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IEvent[] ClearDomainEvent()
    {
        IEvent[] listDomainEvent = _domainEvents.ToArray();
        _domainEvents.Clear();
        return listDomainEvent;
    }
}

