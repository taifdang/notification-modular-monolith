using API.Notification.Models;
using ShareCommon.DTO;
using ShareCommon.Model;

namespace API.Notification.Services
{
    public interface IEventHandler
    {
        string event_type { get; }
        Task HandleAsync(InboxNotification message);//action
    }
}
