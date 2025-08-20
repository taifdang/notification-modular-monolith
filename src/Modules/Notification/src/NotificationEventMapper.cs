using BuildingBlocks.Core;
using BuildingBlocks.Core.Event;
using Notification.Notifications.Features.CreatingNotification;

namespace Notification;
public class NotificationEventMapper : IEventMapper
{
    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            _ => null
        };
    }

    public IInternalCommand? MapToInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {
            PersonalNotificationCreatedDomainEvent e => new CreatePersonalNotification(e.Id,e.UserId),
            _ => null
        };
    }
}
