using Notification.Messages.Model;

namespace Notification.Messages;

public interface INotificationMessageRepository
{
    void Add(Message message);
}
