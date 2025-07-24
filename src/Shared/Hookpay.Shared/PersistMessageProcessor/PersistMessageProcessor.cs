

using Hookpay.Shared.Domain.Events;
using Hookpay.Shared.Utils;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Hookpay.Shared.PersistMessageProcessor;

public class PersistMessageProcessor : IPersistMessageProcessor
{
    private readonly ILogger<PersistMessageProcessor> _logger;
    private readonly IMediator _mediator;
    private readonly IPersistMessageDbContext _persistMessageDbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public PersistMessageProcessor(
        ILogger<PersistMessageProcessor> logger,
        IMediator mediator,
        IPersistMessageDbContext persistMessageDbContext,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _mediator = mediator;
        _persistMessageDbContext = persistMessageDbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishMessageAsync<T>(
        T messageEnvelope, 
        CancellationToken cancellationToken = default) 
        where T : MessageEnvelope
    {
        await SavePersistMessageAsync(messageEnvelope, MessageDeliveryType.Outbox, cancellationToken);
    }

    public async Task AddInternalMessageAsync<T>(
        T internalCommand,
        CancellationToken cancellationToken)
        where T : class, IInternalCommand
    {
        await SavePersistMessageAsync(new MessageEnvelope(internalCommand), MessageDeliveryType.Internal, cancellationToken);
    }

    private async Task<Guid> SavePersistMessageAsync(
        MessageEnvelope messageEnvelope, 
        MessageDeliveryType deliveryType, 
        CancellationToken cancellationToken = default)
    {
        if (messageEnvelope.Message == null)
        {
            throw new Exception("message is empty");
        }

        Guid id;

        if (messageEnvelope.Message is IEvent message)
            id = message.eventId;
        else
            id = NewId.NextGuid();

        //save event
        await _persistMessageDbContext.PersistMessage.AddAsync(
            new PersistMessage(
                id,
                messageEnvelope.Message.GetType().ToString(),
                JsonSerializer.Serialize(messageEnvelope),
                deliveryType
                ),
            cancellationToken
            );

        await _persistMessageDbContext.SaveChangesAsync();

        _logger.LogInformation($"Message with id: {id} and deliveryType = {deliveryType.ToString()} saved.");

        return id;
    }

    public async Task ProcessAllAsync(CancellationToken cancellationToken = default)
    {
        var messages = await _persistMessageDbContext.PersistMessage
            .Where(x => x.MessageStatus != MessageStatus.Processed)
            .ToListAsync(cancellationToken);

        foreach (var message in messages )
        {
            await ProcessAsync(message.Id, message.DeliveryType, cancellationToken);
        }
    }

    public async Task ProcessAsync(Guid MessageId, MessageDeliveryType deliveryType, CancellationToken cancellationToken = default)
    {
        //find message
        var message = await _persistMessageDbContext.PersistMessage
            .FirstOrDefaultAsync(x => x.Id  == MessageId && x.DeliveryType == deliveryType, cancellationToken);

        if (message is null)
            return;

        switch(deliveryType)
        {
            case MessageDeliveryType.Internal:
                var senInternal = await ProcessInternalAsync(message, cancellationToken);
                if (senInternal)
                {
                    await ChangeMessageStatusAsync(message, cancellationToken);
                    break;
                }
                else
                {
                    return;
                }

            case MessageDeliveryType.Outbox:

                var sentOutbox = await ProcessOutboxAsync(message, cancellationToken);
                if (sentOutbox)
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

    private async Task ChangeMessageStatusAsync(PersistMessage message, CancellationToken cancellationToken = default)
    {
        message.ChangeState(MessageStatus.Processed);

        _persistMessageDbContext.PersistMessage.Update(message);

        await _persistMessageDbContext.SaveChangesAsync(cancellationToken);
    }
    

    private async Task<bool> ProcessOutboxAsync(PersistMessage message, CancellationToken cancellationToken)
    {        
        var messageEnvelope = JsonSerializer.Deserialize<MessageEnvelope>(message.Data);

        if (messageEnvelope is null || messageEnvelope.Message is null)
            return false;

        var data = JsonSerializer.Deserialize(
            messageEnvelope.Message?.ToString() ?? string.Empty,
            TypeProvider.GetTypeFromAssembly(message.DataType) ?? typeof(object));

        if (data is not IEvent)
            return false;

        await _publishEndpoint.Publish(data, context =>
        {
            foreach (var header in messageEnvelope.Headers)
                context.Headers.Set(header.Key, header.Value);
        }, cancellationToken);

        _logger.LogInformation(
            "Message with id: {MessageId} and deliveryType: {DeleveryType} is processed",
            message.Id,
            message.DeliveryType
            );

        return true;

    }

    public async Task<bool> ProcessInternalAsync(
        PersistMessage message, 
        CancellationToken cancellationToken = default)
    {
        var messageEnvelope = JsonSerializer.Deserialize<MessageEnvelope>(message.Data);

        if (messageEnvelope is null || messageEnvelope.Message is null)
            return false;

        var data = JsonSerializer.Deserialize(
            messageEnvelope.GetType().ToString() ?? string.Empty,
            TypeProvider.GetTypeFromAssembly(message.DataType) ?? typeof(object)
            );

        if (data is not IInternalCommand internalCommand)
            return false;

        await _mediator.Send(internalCommand, cancellationToken);

        _logger.LogInformation(
            "InternalCommand with id: {MessageId} and deliveryType: {DeleveryType} is processed",
            message.Id,
            message.DeliveryType
            );

        return true;
    }

    public Task<PersistMessage> ExistMessageAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        return _persistMessageDbContext.PersistMessage
            .FirstOrDefaultAsync(
                x=>x.Id == messageId && 
                x.DeliveryType == MessageDeliveryType.Inbox &&
                x.MessageStatus == MessageStatus.Processed,
                cancellationToken);

    }

    public Task<Guid> AddReceivedMessageAsync<T>(
        T messageEnvelope,
        CancellationToken cancellationToken = default) 
        where T : MessageEnvelope
    {
        return SavePersistMessageAsync(messageEnvelope , MessageDeliveryType.Inbox, cancellationToken);
    }

    public async Task ProcessInboxAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var message = await _persistMessageDbContext.PersistMessage
            .FirstOrDefaultAsync(x =>
                x.Id == messageId &&
                x.DeliveryType == MessageDeliveryType.Inbox &&
                x.MessageStatus == MessageStatus.InProgress,
                cancellationToken                   
            );

        await ChangeMessageStatusAsync(message, cancellationToken);
    }
}
