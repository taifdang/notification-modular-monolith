
namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;

public interface ICreateMessageProcessor
{
    Task LoadCacheDataAsync(CancellationToken cancellationToken = default);
    Task AddAllMessageAsync(
        string message,
        CancellationToken cancellationToken = default);
    Task AddPersonalMessageAsync(CancellationToken cancellationToken = default);
    Task PublishAllAsync(CancellationToken cancellationToken = default);
    Task PublishAsync(
        int userId, 
        string message,
        CancellationToken cancellationToken = default);
}
