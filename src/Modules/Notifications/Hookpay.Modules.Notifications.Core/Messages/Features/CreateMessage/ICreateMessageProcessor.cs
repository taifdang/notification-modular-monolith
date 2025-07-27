

namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;

public interface ICreateMessageProcessor
{
    Task AddAllMessageAsync(
        string data,
        CancellationToken cancellationToken = default);
    Task AddPersonalMessageAsync(
        int userId,
        string data,
        CancellationToken cancellationToken = default);
    Task ProcessAllAsync<T>(
        IReadOnlyList<T> ListUser,
        string data,
        CancellationToken cancellationToken = default);
    Task ProcessAsync(
        int userId,
        string data,  
        CancellationToken cancellationToken = default);
    Task SaveStatePublishMessage(CancellationToken cancellationToken = default);
}
