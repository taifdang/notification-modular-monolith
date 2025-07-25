﻿
using Hookpay.Shared.Core.Events;

namespace Hookpay.Shared.PersistMessageProcessor;

public interface IPersistMessageProcessor
{
    public Task PublishMessageAsync<T>(
        T messageEnvelope,
        CancellationToken cancellationToken = default)
        where T : MessageEnvelope;

    public Task AddInternalMessageAsync<T>(
        T internalCommand,
        CancellationToken cancellationToken = default)
        where T : class, IInternalCommand;

    public Task ProcessAllAsync(     
        CancellationToken cancellationToken = default);

    public Task ProcessAsync(
        Guid MessageId,
        MessageDeliveryType deliveryType,
        CancellationToken cancellationToken = default);

    public Task<bool> ProcessInternalAsync(
        PersistMessage message,
        CancellationToken cancellationToken = default);

    Task<PersistMessage> ExistMessageAsync(
        Guid messageId,
        CancellationToken cancellationToken = default);

    Task<Guid> AddReceivedMessageAsync<T>(
        T messageEnvelope,
        CancellationToken cancellationToken = default)
        where T : MessageEnvelope;

    Task ProcessInboxAsync(
       Guid messageId,
       CancellationToken cancellationToken = default);
}
