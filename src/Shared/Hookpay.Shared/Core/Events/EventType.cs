namespace Hookpay.Shared.Core.Events;

[Flags]
public enum EventType
{   
    DomainEvent = 1,
    InteragationEvent = 2,
    InternalCommand = 3,
}
