using BuildingBlocks.Contracts;
using MassTransit;

namespace Notification.Integration.Consumers.IntegrationHandler;
public class BroadcastNotificationHandler : IConsumer<BroadcastNotificationCreated>
{
    public Task Consume(ConsumeContext<BroadcastNotificationCreated> context)
    {
        throw new NotImplementedException();
    }
}
