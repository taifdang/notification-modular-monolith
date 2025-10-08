using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Event;
using Notification.Notifications.Features.GettingNotificationById;


namespace Notification;
public sealed class NotificationEventMapper : IEventMapper
{
    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent @event)
    {
        return @event switch
        {
            PersonalNotificationCreatedDomainEvent e => new NotificationCreated(e.Id, e.UserId, e.email),
            _ => null
        };
    }

    public IInternalCommand? MapToInternalCommand(IDomainEvent @event)
    {
        return @event switch
        {
            //PersonalNotificationCreatedDomainEvent e => new NotificationReceived(e.Id,e.UserId,e.email),
            _ => null
        };
    }
}
