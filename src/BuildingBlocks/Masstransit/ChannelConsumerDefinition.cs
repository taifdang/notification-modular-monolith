using MassTransit;

namespace BuildingBlocks.Masstransit;

public abstract class ChannelConsumerDefinition<TConsumer> : ConsumerDefinition<TConsumer>
    where TConsumer : class, IConsumer
{
    private readonly string _routingKey;
   // private readonly string _endpoint;
    protected ChannelConsumerDefinition(string endpoint, string routingKey)
    {
        EndpointName = endpoint;
        _routingKey = routingKey;
    }
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, 
        IConsumerConfigurator<TConsumer> consumerConfigurator)
    {
        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            //endpointConfigurator.EndpointName = _endpoint;
            endpointConfigurator.ConfigureConsumeTopology = false;
            rmq.Bind("notification-exchange", s =>
            {
                s.RoutingKey = _routingKey;
                s.ExchangeType = "topic";
            });
        }
    }
}
