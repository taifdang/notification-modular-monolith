using BuildingBlocks.Masstransit;
using MassTransit;
using Notification.Infrastructure.Messages.Consumers.Sending.InApp;
using Notification.Infrastructure.Messages.Contracts;
using Notification.Infrastructure.Services;

namespace Notification.Infrastructure.Extensions;

public class MasstransitExtensions : IMasstransitModule
{
    public void ConfigureTopology(IBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        cfg.Message<NotificationDispatched>(x =>
        {
            x.SetEntityName("notification-exchange");
        });

        cfg.ReceiveEndpoint("inapp-queue", e =>
        {
            if (e is IRabbitMqReceiveEndpointConfigurator rmq)
            {
                e.ConfigureConsumeTopology = false;

                rmq.Bind("notification-exchange", s =>
                {
                    s.RoutingKey = "inapp";
                    s.ExchangeType = "topic";
                });
            }
            e.ConfigureConsumer<InappConsumer>(context);
        });
    }
}
