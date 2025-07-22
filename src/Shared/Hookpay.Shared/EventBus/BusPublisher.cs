using MassTransit;

namespace Hookpay.Shared.EventBus;

public class BusPublisher : IBusPublisher
{
    private readonly IPublishEndpoint _publisher;
    public BusPublisher(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }
   
    public async Task SendAsync<T>(T IntegrationEvent, CancellationToken cancellationToken = default)
    {
        await _publisher.Publish(IntegrationEvent, cancellationToken);
    }
}
