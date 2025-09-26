using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Event;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace UserProfile.Topups.Consumers.CreatingTopup;
public class CreateTopup : IConsumer<TopupCreated>
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly ILogger<CreateTopup> _logger;

    public CreateTopup(IEventDispatcher eventDispatcher, ILogger<CreateTopup> logger)
    {
        _eventDispatcher = eventDispatcher;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<TopupCreated> context)
    {

        Guard.Against.Null(context.Message, nameof(TopupCreated));

        _logger.LogInformation($"consumer for {nameof(TopupCreated)} is processing");

        var @event = context.Message;

        await _eventDispatcher.SendAsync(
            new TopupCreatedDomainEvent(@event.Id, @event.UserName, @event.TransferAmount),
            typeof(IInternalCommand));
    }
}
