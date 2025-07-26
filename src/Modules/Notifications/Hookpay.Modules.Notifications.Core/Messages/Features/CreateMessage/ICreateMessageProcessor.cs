
using Hookpay.Modules.Notifications.Core.Messages.Enums;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;

public interface ICreateMessageProcessor
{
    Task MessageLoadingProcessor(
        string? message,
        MessageProcessorType processorType,       
        CancellationToken cancellationToken = default);
    Task AddAllMessageAsync(
        string message,
        PushType pushType,
        CancellationToken cancellationToken = default);
    Task AddPersonalMessageAsync(
        int userId,
        string message,
        PushType pushType,
        CancellationToken cancellationToken = default);
    Task PublishAllAsync<T>(
        IReadOnlyList<T> ListUser,
        string message,
        PushType pushType,
        CancellationToken cancellationToken = default);
    Task PublishAsync(
        int userId, 
        string message,
        PushType pushType,  
        CancellationToken cancellationToken = default);
    Task SaveStatePublishMessage(CancellationToken cancellationToken = default);
}
