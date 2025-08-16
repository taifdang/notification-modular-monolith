using Notification.Recipients.Model;

namespace Notification.Recipients;

public interface INotificationRecipientRepository
{
    void Add(Recipient recipient);
}
