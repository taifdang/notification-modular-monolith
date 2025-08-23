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

    public async Task ChangeNotificationStatusAsync(NotificationDelivery notificationDelivery,
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
        var rules = false;
        if (!rules)
        {
            //save in UserFeed
        }

        switch (channelType)
        {
            case ChannelType.InApp:
                var sentInappChannel = await ProcessInappAsync(notification, cancellationToken); 
                if (sentInappChannel)
                {
                    await ChangeNotificationStatusAsync(notification, cancellationToken);
                    break;
                }
                else
                {
                    return;
                }
            case ChannelType.Email:
                var sentEmailChannel = await ProcessEmailAsync(notification, cancellationToken);
                if (sentEmailChannel)
                {
                    await ChangeNotificationStatusAsync(notification, cancellationToken);
                    break;
                }
                else
                {
                    return;
                }
        }
    }

    private async Task<bool> ProcessEmailAsync(NotificationDelivery notificationDelivery,
        CancellationToken cancellationToken = default)
    {
        //save message in UserFeed
        return true;
    }

    private async Task<bool> ProcessInappAsync(NotificationDelivery notificationDelivery,
        CancellationToken cancellationToken = default)
    {      
        //save message in UserFeed
        return true;
    }
}
