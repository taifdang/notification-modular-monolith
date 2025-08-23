using BuildingBlocks.Contracts;
using MassTransit;

namespace Notification.PersistNotificationProcessor.Consumers.ReceiveNewRequest;
public class BroadcastNotificationHandler : IConsumer<BroadcastNotificationRequested>
{
    public Task Consume(ConsumeContext<BroadcastNotificationRequested> context)
    {
        throw new NotImplementedException();
    }
}
