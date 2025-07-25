using Hookpay.Shared.Domain.Events;

namespace Hookpay.Shared.Domain.Models;

public abstract class Aggregate : Aggregrate<int>
{
}
public abstract class Aggregrate<T> : Entity, IAggregate<T>
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public int Version { get; set; } = -1;

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

