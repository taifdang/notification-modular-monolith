


using BuildingBlocks.Core;
using BuildingBlocks.Core.Event;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        throw new NotImplementedException();
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



    public Task ProcessInboxAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task PublishMessageAsync<TMessageEnvelope>(
        TMessageEnvelope messageEnvelop,
        CancellationToken cancellationToken = default)
        where TMessageEnvelope : MessageEnvelope
    {
        throw new NotImplementedException();
    }
 

     public Task AddInternalMessageAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
         where TCommand : class, IInternalCommand
     {
        throw new NotImplementedException();
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
    private Task<bool> ProcessInternalAsync(PersistMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private Task<bool> ProcessOutboxAsync(PersistMessage message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task ChangeMessageStatusAsync(PersistMessage message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    #endregion
}
