
using MediatR;

namespace BuildingBlocks.Core.Event;

public interface IEvent : INotification
{
    Guid eventId => Guid.NewGuid();
    public DateTime createAt => DateTime.Now;

    //ref: https://learn.microsoft.com/en-us/dotnet/api/system.type.assemblyqualifiedname?view=net-9.0
    public string eventType => GetType().AssemblyQualifiedName;
}
