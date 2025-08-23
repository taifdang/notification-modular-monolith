using BuildingBlocks.Contracts;
using Notification.NotificationDeliveries.Model;

namespace Notification.NotificationDeliveries.Features.PersistNotification;
public interface IPersistNotificationProcessor
{
    Task ProcessAllAsync(CancellationToken cancellationToken = default);
    Task ProcessAsync(Guid Id, ChannelType channelType, CancellationToken cancellationToken = default);
    Task ChangeNotificationStatusAsync(NotificationDelivery notificationDelivery,
        CancellationToken cancellationToken = default);
}
