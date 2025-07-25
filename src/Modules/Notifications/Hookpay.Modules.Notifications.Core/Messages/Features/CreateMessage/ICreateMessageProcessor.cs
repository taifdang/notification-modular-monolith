
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
        CancellationToken cancellationToken = default);
    Task AddPersonalMessageAsync(
        int userId,
        string message,
        CancellationToken cancellationToken = default);
    Task PublishAllAsync<T>(
        IReadOnlyList<T> listUser,
        string message,
        CancellationToken cancellationToken = default);
    Task PublishAsync(
        int userId, 
        string message,
        CancellationToken cancellationToken = default);
    Task SaveStatePublishMessage(CancellationToken cancellationToken = default);
}
