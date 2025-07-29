

using BuildingBlocks.Core.Event;

namespace BuildingBlocks.PersistMessageProcessor;

//ref: https://github.com/kgrzybek/modular-monolith-with-ddd?tab=readme-ov-file#37-modules-integration
public interface IPersistMessageProcessor
{
    Task PublishMessageAsync<TMessageEnvelope>(
        TMessageEnvelope messageEnvelop,
        CancellationToken cancellationToken = default)
        where TMessageEnvelope : MessageEnvelope;

    Task<Guid> AddReceiveMessage<TMessageEnvelope>(
        TMessageEnvelope messageEnvelope,
        CancellationToken cancellationToken = default)
        where TMessageEnvelope : MessageEnvelope;

    Task AddInternalMessage<TCommand>(
        TCommand command,
        CancellationToken cancellationToken = default)
        where TCommand : class, IInternalCommand;

    Task ProcessInboxMessaage(
        Guid messageId,
        CancellationToken cancellationToken = default);

    Task ExistMessageAsync(
        Guid messageId,
        CancellationToken cancellationToken = default);

    Task ProcessAllAsync(CancellationToken cancellationToken = default);

    Task ProcessAsync(
        Guid messageId,
        MessageDeliveryType deliveryType,
        CancellationToken cancellationToken = default
        );      
}
