


namespace BuildingBlocks.PersistMessageProcessor;

public class PersistMessageProcessor : IPersistMessageProcessor
{
    public Task<Guid> AddReceiveMessage<TMessageEnvelope>(TMessageEnvelope messageEnvelope, CancellationToken cancellationToken = default) where TMessageEnvelope : MessageEnvelope
    {
        throw new NotImplementedException();
    }

    public Task ExistMessageAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ProcessAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ProcessAsync(Guid messageId, MessageDeliveryType deliveryType, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ProcessInboxMessaage(Guid messageId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task PublishMessageAsync<TMessageEnvelope>(TMessageEnvelope messageEnvelop, CancellationToken cancellationToken = default) where TMessageEnvelope : MessageEnvelope
    {
        throw new NotImplementedException();
    }

    Task IPersistMessageProcessor.AddInternalMessage<TCommand>(TCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
