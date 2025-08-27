using BuildingBlocks.Contracts;
using MassTransit;
using Notification.Data;

namespace Notification.PersistNotificationProcessor.Consumers.ReceiveNewRequest;
public class BroadcastNotificationHandler : IConsumer<BroadcastNotificationRequested>
{
    public Task Consume(ConsumeContext<BroadcastNotificationRequested> context)
    {
        throw new NotImplementedException();
    }
}
