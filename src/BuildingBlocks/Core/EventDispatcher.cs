

using BuildingBlocks.Core.Event;
using BuildingBlocks.PersistMessageProcessor;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BuildingBlocks.Core;

using EventType = Event.EventType;
public sealed class EventDispatcher(
    IEventMapper eventMapper,
    ILogger<EventDispatcher> logger,
    IPersistMessageProcessor persistMessageProcessor,
    IHttpContextAccessor httpContextAccessor) : IEventDispatcher
{

    public async Task SendAsync<T>(
        IReadOnlyList<T> events, 
        Type type = null, 
        CancellationToken cancellationToken = default) 
        where T : IEvent
    {
        if(events.Count > 0)
        {
            var eventType = type != null && type.IsAssignableTo(typeof(IInternalCommand))
                ? EventType.InternalCommand
                : EventType.DomainEvent;

            async Task PublishIntegrationEvent(IReadOnlyList<IIntegrationEvent> integrationEvents)
            {
                foreach(var integrationEvent in integrationEvents)
                {
                    await persistMessageProcessor.PublishMessageAsync(
                        new MessageEnvelope(
                            integrationEvent,
                            SetHeaders()),
                        cancellationToken);
                }
            }

            switch (events) 
            { 
               case IReadOnlyList<IDomainEvent> domainEvents:

                    {
                        var integrationEvents = await MapDomainEventToIntegrationEvent(domainEvents)
                        .ConfigureAwait(false);

                        await PublishIntegrationEvent(integrationEvents);
                        break;
                    }

               case IReadOnlyList<IIntegrationEvent> integrationEvents:

                    await PublishIntegrationEvent(integrationEvents);
                    break;
            }

            if(type != null && eventType == EventType.InternalCommand)
            {
                var internalMessages = await MapDomainEventToInternalCommand(events as IReadOnlyList<IDomainEvent>);

                foreach(var internalMessage in internalMessages)
                {
                    await persistMessageProcessor.AddInternalMessage(internalMessage, cancellationToken);
                }
            }

        }
    }

    public async Task SendAsync<T>(
        T @event, 
        Type type = null,
        CancellationToken cancellationToken = default)
        where T : IEvent
    {
        await SendAsync(new[] { @event }, type, cancellationToken);
    }

    #region Internal Method


    public Task<IReadOnlyList<IIntegrationEvent>> MapDomainEventToIntegrationEvent(IReadOnlyList<IDomainEvent> events)
    {
        var wrapperIntegrationEvents = GetWrappedIntegrationEvents(events.ToList())?.ToList();

        if (wrapperIntegrationEvents?.Count > 0)
            return Task.FromResult<IReadOnlyList<IIntegrationEvent>>(wrapperIntegrationEvents);

        var integrationEvents = new List<IIntegrationEvent>();

        foreach(var @event in events)
        {
            var eventType = @event.GetType();

            logger.LogTrace($"Handler domain event :{eventType.Name}");

            var integrationEvent = eventMapper.MapToIntegrationEvent(@event);

            if (integrationEvent is null)
                continue;

            integrationEvents.Add(integrationEvent);
        }

        return Task.FromResult<IReadOnlyList<IIntegrationEvent>>(integrationEvents);
    }

    public Task<IReadOnlyList<IInternalCommand>> MapDomainEventToInternalCommand(IReadOnlyList<IDomainEvent> events)
    {
        
        var internalCommands = new List<IInternalCommand>();

        foreach(var @event in events)
        {
            var eventType = @event.GetType();

            logger.LogTrace($"Handler domain event: {eventType.Name}");

            var integrationEvent = eventMapper.MapToInternalCommand(@event);

            if(integrationEvent is null)
                continue;

            internalCommands.Add(integrationEvent);
        }
        return Task.FromResult<IReadOnlyList<IInternalCommand>>(internalCommands);

    }

    //ref: https://learn.microsoft.com/en-us/dotnet/api/system.activator.createinstance?view=net-9.0
    //ref: https://readmedium.com/domain-driven-design-domain-events-and-integration-events-in-net-5a2a58884aaa
    private IEnumerable<IIntegrationEvent> GetWrappedIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents.Where(x => x is IHaveIntegrationEvent))
        {
            var genericType = typeof(IntegrationEventWrapper<>)
                .MakeGenericType(domainEvent.GetType());

            var domainNotificationEvent = (IIntegrationEvent)Activator
                .CreateInstance(genericType, domainEvent);

            yield return domainNotificationEvent;
        }
    }

    private IDictionary<string, object> SetHeaders()
    {
        var headers = new Dictionary<string, object>();

        headers.Add("CorrelationId", httpContextAccessor?.HttpContext?.GetCorrelationId());
        headers.Add("UserId", httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
        headers.Add("UserName", httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name));

        return headers;
    }

    #endregion
}
