using BuildingBlocks.Contracts;
using MassTransit;

namespace Notification.Integration.Consumers.IntegrationHandler;
public class BroadcastNotificationHandler : IConsumer<BroadcastNotificationRequested>
{
    public Task Consume(ConsumeContext<BroadcastNotificationRequested> context)
    {
        throw new NotImplementedException();
    }
}
