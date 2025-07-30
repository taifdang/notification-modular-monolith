using BuildingBlocks.Core.Event;
using BuildingBlocks.Utils;
using MassTransit;
using MassTransit.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BuildingBlocks.PersistMessageProcessor;

public class PersistMessageProcessor : IPersistMessageProcessor
{
    private readonly ILogger<PersistMessageProcessor> _logger;  
    private readonly IPersistMessageDbContext _persistMessageDbContext;
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;

    public PersistMessageProcessor(
        ILogger<PersistMessageProcessor> logger,
        IPersistMessageDbContext persistMessageDbContext,
        IMediator mediator,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _persistMessageDbContext = persistMessageDbContext;
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    public Task<Guid> AddReceiveMessageAsync<TMessageEnvelope>(
        TMessageEnvelope messageEnvelope,
        CancellationToken cancellationToken = default) 
        where TMessageEnvelope : MessageEnvelope
    {
        return SavePersistMessageAsync(
            messageEnvelope,
            MessageDeliveryType.Inbox,
            cancellationToken);
    }

    public async Task ProcessAllAsync(CancellationToken cancellationToken = default)
    {
        var processes = await _persistMessageDbContext.PersistMessage
            .Where(x => x.MessageStatus != MessageStatus.Processed)
            .ToListAsync();

        foreach (var process in processes)
        {
            await ProcessAsync(
                process.Id,
                process.DeliveryType,
                cancellationToken);
        }
    }

    public async Task ProcessAsync(
        Guid messageId,
        MessageDeliveryType deliveryType,
        CancellationToken cancellationToken = default)
    {
        var message = await _persistMessageDbContext.PersistMessage.
            FirstOrDefaultAsync(x =>
                    x.Id  == messageId &&
                    x.DeliveryType == deliveryType,
                cancellationToken);

        if (message is null)
            return;

        switch (deliveryType)
        {
            case MessageDeliveryType.Internal:
                var sentInternalMessage = await ProcessInternalAsync(message, cancellationToken);
                if (sentInternalMessage)
                {
                    await ChangeMessageStatusAsync(message, cancellationToken);
                    break;
                }
                else
                {
                    return;
                }


            case MessageDeliveryType.Outbox:
                var sentOutboxMessage = await ProcessOutboxAsync(message, cancellationToken);
                if (sentOutboxMessage)
                {
                    await ChangeMessageStatusAsync(message, cancellationToken);
                    break;
                }
                else
                {
                    return;
                }
        }
            
    }

    public async Task ProcessInboxAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var message = await _persistMessageDbContext.PersistMessage.FirstOrDefaultAsync(
            x => x.Id == messageId &&
                 x.DeliveryType == MessageDeliveryType.Inbox &&
                 x.MessageStatus == MessageStatus.InProgress,
            cancellationToken
            );

        await ChangeMessageStatusAsync(message, cancellationToken);
    }

    public async Task PublishMessageAsync<TMessageEnvelope>(
        TMessageEnvelope messageEnvelope,
        CancellationToken cancellationToken = default)
        where TMessageEnvelope : MessageEnvelope
    {
        await SavePersistMessageAsync(messageEnvelope, MessageDeliveryType.Outbox, cancellationToken);
    }
 

     public async Task AddInternalMessageAsync<TCommand>(
         TCommand command, 
         CancellationToken cancellationToken)
         where TCommand : class, IInternalCommand
     {
        await SavePersistMessageAsync(
            new MessageEnvelope(command),
            MessageDeliveryType.Internal,
            cancellationToken);
     }

    public async Task<PersistMessage> ExistMessageAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        return await _persistMessageDbContext.PersistMessage
            .FirstOrDefaultAsync(x => 
                    x.Id == messageId &&
                    x.DeliveryType == MessageDeliveryType.Inbox &&
                    x.MessageStatus == MessageStatus.Processed, 
                cancellationToken);
    }

    #region Internal Method
    private async Task<bool> ProcessInternalAsync(PersistMessage message, CancellationToken cancellationToken = default)
    {
        var messageEnvelope = JsonSerializer.Deserialize<MessageEnvelope>(message.Data);

        if (messageEnvelope is null || messageEnvelope.Message is null)
            return false;

        var data = JsonSerializer.Deserialize(
            messageEnvelope.Message?.ToString() ?? string.Empty,
            TypeProvider.GetTypeFromCurrentDomainAssembly(message.DataType) ?? typeof(object));

        if(data is not IInternalCommand internalCommand)
            return false;

        await _mediator.Send(internalCommand, cancellationToken);

        _logger.LogInformation(
            "InternalCommand with id: {EventId} and deliveryType: {DeliveryType} processed from the persistent message processor",
            message.Id,
            message.DeliveryType);

        return true;
    }

    private async Task<bool> ProcessOutboxAsync(PersistMessage message, CancellationToken cancellationToken = default)
    {
        var messageEnvelope = JsonSerializer.Deserialize<MessageEnvelope>(message.Data);

        if (messageEnvelope is null || messageEnvelope.Message is null)
            return false;

        var data = JsonSerializer.Deserialize(
            messageEnvelope.Message?.ToString() ?? string.Empty,
            TypeProvider.GetTypeFromCurrentDomainAssembly(message.DataType) ?? typeof(object));

        if (data is not IEvent)
            return false;

        await _publishEndpoint.Publish(data, context=>
        {
            //set headers in queue
            foreach(var header in messageEnvelope.Headers)
                context.Headers.Set(header.Key, header.Value);

        },cancellationToken);

        _logger.LogInformation(
            "Message with id: {EventId} and deliveryType: {DeliveryType} processed from the persistent message processor",
            message.Id,
            message.DeliveryType);

        return true;

    }

    private async Task<Guid> SavePersistMessageAsync(
        MessageEnvelope messageEnvelope,
        MessageDeliveryType deliveryType,
        CancellationToken cancellationToken = default)
    {
        //?
        if (messageEnvelope.Message is null)
            return default!;

        Guid id;
        if (messageEnvelope.Message is IEvent message)
            id = message.eventId;
        else
            id = Guid.NewGuid();

        await _persistMessageDbContext.PersistMessage.AddAsync( 
            new PersistMessage
            (
                id,
                messageEnvelope.Message.GetType().ToString(),
                JsonSerializer.Serialize(messageEnvelope),
                deliveryType
            ), 
            cancellationToken);

        await _persistMessageDbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Message with id: {EventId} and deliveryType: {DeliveryType} saved in the persistent message processor",
            id,
            deliveryType.ToString());

        return id;

    }

    private async Task ChangeMessageStatusAsync(PersistMessage message, CancellationToken cancellationToken)
    {
        message.ChangeState(MessageStatus.Processed);

        _persistMessageDbContext.PersistMessage.Update(message);

        await _persistMessageDbContext.SaveChangesAsync();
    }

    #endregion
}
