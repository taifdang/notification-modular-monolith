
namespace Hookpay.Shared.SignalR;

public interface INotificationHubService
{
    Task SendAllAsync(string message);
    Task SendPersonalAsync(string userId, string message);
}
