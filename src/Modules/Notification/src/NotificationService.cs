using Notification.Messages;
using Notification.Notifications;
using Notification.Notifications.Model;
using Notification.Recipients;

namespace Notification;

public class NotificationService
{
    private readonly INotificationMessageRepository _messageRepository;
    private readonly INotificationRecipientRepository _recipientRepository;
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationMessageRepository messageRepository, INotificationRecipientRepository recipientRepository,
        INotificationRepository notificationRepository)
    {
        _messageRepository = messageRepository;
        _recipientRepository = recipientRepository;
        _notificationRepository = notificationRepository;
    }

    //public Task CreateNotificationAsync(CancellationToken cancellationToken = default)
    //{
    //    var notification = new Notifications.Model.Notification();

    //}


}
