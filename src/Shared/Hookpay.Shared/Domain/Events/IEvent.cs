using MediatR;

namespace Hookpay.Shared.Domain.Events;

public interface IEvent:INotification
{
    Guid eventId => Guid.NewGuid();
    public DateTime createAt => DateTime.Now;
    public string eventType => GetType().AssemblyQualifiedName;
}
