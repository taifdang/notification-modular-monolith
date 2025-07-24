
using Hookpay.Shared.Domain.Events;
using Hookpay.Shared.PersistMessageProcessor;
using Hookpay.Shared.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hookpay.Shared.Core;

public sealed class EventDispatcher (
    IServiceScopeFactory _serviceScopeFactory,
    ILogger<EventDispatcher> _logger,
    IEventMapper _eventMapper,
    IPersistMessageProcessor _persistMessageProcessor,
    IHttpContextAccessor _httpContextAccessor
    ) : IEventDispatcher
{
    public async Task SendAsync<T>(IReadOnlyList<T> events, Type type = null, CancellationToken cancellationToken = default) where T : IEvent
    {

        if(events.Count() >0)
        {
            //get eventType
            var eventType = type != null && type.IsAssignableTo(typeof(IInternalCommand))
                    ? EventType.InternalCommand
                    : EventType.DomainEvent;


            switch (events)
            {
                case IReadOnlyList<IDomainEvent> domainEvents:

                    //mapping domainEvent into integrationEvent
                    var integrationEvents = await MapDomainEventToIntegrationEvent(domainEvents).ConfigureAwait(false);

                    await PublishIntegrationEventAsync(integrationEvents);

                    break;

                case IReadOnlyList<IIntegrationEvent> integrationEvent:

                    await PublishIntegrationEventAsync(integrationEvent);

                    break;
            }

            if (type != null && eventType == EventType.InternalCommand)
            {
                var internalCommands = await MapDomainEventToInternalCommand(events as IReadOnlyList<IDomainEvent>)
                    .ConfigureAwait(false);

                foreach (var internalCommand in internalCommands)
                {
                    await _persistMessageProcessor.AddInternalMessageAsync(internalCommand, cancellationToken);
                }
            }
        }

    }

    public async Task SendAsync<T>(T @event, Type type = null, CancellationToken cancellationToken = default) 
        where T : IEvent
    {
        await SendAsync(new[] {@event}, type, cancellationToken);
    }

    public async Task PublishIntegrationEventAsync(
        IReadOnlyList<IIntegrationEvent> integrationEvents, CancellationToken cancellationToken = default)
    {
        foreach(var integrationEvent in integrationEvents)
        {
            await _persistMessageProcessor.PublishMessageAsync(
                new MessageEnvelope(integrationEvent, SetHeaders()),
                cancellationToken
            );
        }       
    }

    //private
    private Task<IReadOnlyList<IIntegrationEvent>> MapDomainEventToIntegrationEvent(
        IReadOnlyList<IDomainEvent> domainEvents)
    {
        _logger.LogTrace("Processing integration event start ...");

        var wrappedIntegrationEvent =  GetWrappedIntegrationEvent(domainEvents.ToList())?.ToList();

        if(wrappedIntegrationEvent?.Count() > 0)
        {
            return Task.FromResult<IReadOnlyList<IIntegrationEvent>>(wrappedIntegrationEvent);
        }

        var integrationEvents = new List<IIntegrationEvent>();

        //using var scope = _serviceScopeFactory.CreateScope();

        foreach(var domainEvent in domainEvents)
        {
            var eventType = domainEvent.GetType();

            _logger.LogTrace($"Handling domain event: {eventType.Name}");

            var integrationEvent = _eventMapper.MapIntegrationEvent(domainEvent);

            if (integrationEvent is null)
                continue;

            integrationEvents.Add(integrationEvent);
        }

        _logger.LogTrace("Processing integration event done ...");

        return Task.FromResult<IReadOnlyList<IIntegrationEvent>>(integrationEvents);
    }

    public Task<IReadOnlyList<IInternalCommand>> MapDomainEventToInternalCommand(IReadOnlyList<IDomainEvent> @events)
    {
        _logger.LogTrace("Processing internal command start ...");

        var internalCommands = new List<IInternalCommand>();

        using var scope = _serviceScopeFactory.CreateScope();
        foreach (var @event in @events)
        {
            var eventType = @event.GetType();

            _logger.LogTrace($"Handling domain event: {eventType.Name}");

            var integrationEvent = _eventMapper.MapInternalCommand(@event);

            if(integrationEvent is null)
                continue;

            internalCommands.Add(integrationEvent);
        }

        return Task.FromResult<IReadOnlyList<IInternalCommand>>(internalCommands);
    }

    private IEnumerable<IIntegrationEvent> GetWrappedIntegrationEvent(IReadOnlyList<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents.Where(x => x is IHaveIntegrationEvent))
        {
            Type genericType = typeof(IntegrationEventWrapper<>)
                .MakeGenericType(domainEvent.GetType());    

            IIntegrationEvent integrationEvent = (IIntegrationEvent)Activator
                .CreateInstance(genericType,domainEvent);

            yield return integrationEvent;
        }
       
    }

    private IDictionary<string, object> SetHeaders()
    {
        //_httpContextAccessor?.HttpContext?.User?.FindFirst("uid").Value

        var headers = new Dictionary<string, object>();

        headers.Add("CorrelationId", _httpContextAccessor?.HttpContext?.GetCorrelationId());
        headers.Add("UserId", "1");
        headers.Add("Username", "2");

        return headers;
    }
}
