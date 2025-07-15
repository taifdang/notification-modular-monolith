using Hookpay.Shared.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Domain.Models;
public interface IAggregate:IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents {  get; }
    IEvent[] ClearDomainEvent();
    int Version { get; set; }
}
public interface IAggregate<out T> : IAggregate
{
    //T Id { get; }
}

