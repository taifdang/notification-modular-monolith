
namespace Notification.Application.Common.Interfaces;

public interface INotificationService
{
    Task Get(Guid id);
}
