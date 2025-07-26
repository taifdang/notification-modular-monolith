using Hookpay.Shared.Core.Events;

namespace Hookpay.Shared.Core.Model;
public interface IAggregate:IEntity, IVersion
{
    IReadOnlyList<IDomainEvent> DomainEvents {  get; }
    IEvent[] ClearDomainEvent();
    //int Version { get; set; }
}
public interface IAggregate<out T> : IAggregate
{
    
}

