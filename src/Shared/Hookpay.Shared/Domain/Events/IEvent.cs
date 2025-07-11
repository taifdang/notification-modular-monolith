using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Domain.Events;

public interface IEvent:INotification
{
    Guid eventId => Guid.NewGuid();
    public DateTime createAt => DateTime.Now;
    public string eventType => GetType().AssemblyQualifiedName;
}
