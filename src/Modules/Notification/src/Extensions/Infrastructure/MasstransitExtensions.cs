using BuildingBlocks.Masstransit;
using Notification.ChannelProcessors.Consumers;

namespace Notification.Extensions.Infrastructure;
//ref: https://bartwullems.blogspot.com/2024/07/automating-masstransit-consumer.html
//public class InappChannelConsumerDefinition :
//    ConsumerDefinition<InappChannelProcessor>
//{
//    public InappChannelConsumerDefinition()
//    {
//        EndpointName = "inapp-queue";
//    }

//    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, 
//        IConsumerConfigurator<InappChannelProcessor> consumerConfigurator)
//    {
//        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
//        {
//            endpointConfigurator.ConfigureConsumeTopology = false;

//            rmq.Bind("notification-exchange", s =>
//            {
//                s.RoutingKey = "inapp";
//                s.ExchangeType = "topic";
//            });
//        }
//    }
//}

//public class InappChannelConsumerDefinition : ChannelConsumerDefinition<InappChannelProcessor>
//{
//    public InappChannelConsumerDefinition() 
//        : base("inapp-queue", "inapp")
//    {
//    }
//}

