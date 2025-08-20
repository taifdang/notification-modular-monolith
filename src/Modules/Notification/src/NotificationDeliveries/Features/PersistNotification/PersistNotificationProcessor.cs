using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notification.Data;
using Notification.NotificationDeliveries.Model;

namespace Notification.NotificationDeliveries.Features.PersistNotification;
public class PersistNotificationProcessor : IPersistNotificationProcessor
{
    private readonly NotificationDbContext _notificationDbContext;
    private readonly ILogger<PersistNotificationProcessor> _logger;
    private readonly IPublishEndpoint _publisherEndpoint;

    public PersistNotificationProcessor(NotificationDbContext notificationDbContext,ILogger<PersistNotificationProcessor> logger,
        IPublishEndpoint publishEndpoint)
    {
        _notificationDbContext = notificationDbContext;
        _logger = logger;
        _publisherEndpoint = publishEndpoint;
    }

    public async Task ChangePersistNotificationAsync(NotificationDelivery notificationDelivery,
        CancellationToken cancellationToken = default)
    {
        notificationDelivery.ChangeState(Enums.DeliveryStatus.Processed);
        _notificationDbContext.NotificationDeliveries.Update(notificationDelivery);
        await _notificationDbContext.SaveChangesAsync();
    }

    public async Task ProcessAllAsync(CancellationToken cancellationToken = default)
    {
        var notifications = _notificationDbContext.NotificationDeliveries
            .Where(x => x.DeliveryStatus == Enums.DeliveryStatus.InProgress && !x.IsDeleted)
            .ToList();

        foreach(var item in notifications)
        {
            await ProcessAsync(item.Id, item.ChannelType);
        }
    }

    public async Task ProcessAsync(Guid Id, ChannelType channelType,
        CancellationToken cancellationToken = default)
    {
        var notification = await _notificationDbContext.NotificationDeliveries
            .FirstOrDefaultAsync(x => x.Id == Id && x.ChannelType == channelType,cancellationToken);

        if (notification is null)
            return;

        //check rule

        

        switch(channelType)
        {
            case ChannelType.InApp:
                var sentInappChannel = await ProcessInappAsync(); 
                if (sentInappChannel)
                {
                    break;
                }
                else
                {
                    return;
                }
        }

        //await _publisherEndpoint.Publish(notification.Message,cancellationToken);
        //await ChangePersistNotificationAsync(notification,cancellationToken);

    }

    private async Task<bool> ProcessInappAsync()
    {
        throw new NotImplementedException();
    }
}
