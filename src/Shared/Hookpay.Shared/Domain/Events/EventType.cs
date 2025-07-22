
namespace Hookpay.Shared.Domain.Events;

[Flags]
public enum EventType
{
    InteragationEvent = 1,
    DomainEvent = 2
}
