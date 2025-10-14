using BuildingBlocks.Masstransit;
using MassTransit;
using Notification.PersistNotificationProcessor.Contracts;
using Notification.PersistNotificationProcessor.Sending.SendingInApp;

namespace Notification.Extensions.Infrastructure;
//ref: https://bartwullems.blogspot.com/2024/07/automating-masstransit-consumer.html

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
            e.ConfigureConsumer<InAppSender>(context);
        });
    }
}
//consumerdefinition if you want to customize the queue/exchange binding
//public class InappChannelConsumerDefinition : BuildingBlocks.Masstransit.ConsumerDefinition<InAppSender>
//{
//    public InappChannelConsumerDefinition()
//        : base("inapp-queue", "inapp", "notification-exchange")
//    {
//    }
//}
