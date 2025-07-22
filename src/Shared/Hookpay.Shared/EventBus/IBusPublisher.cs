
namespace Hookpay.Shared.EventBus;

public interface IBusPublisher
{  
    Task SendAsync<T>(T IntegrationEvent, CancellationToken cancellationToken = default);
}
