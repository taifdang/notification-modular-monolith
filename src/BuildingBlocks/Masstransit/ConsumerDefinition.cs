using MassTransit;

namespace BuildingBlocks.Masstransit;

public abstract class ConsumerDefinition<TConsumer> : MassTransit.ConsumerDefinition<TConsumer>
    where TConsumer : class, IConsumer
{
    private readonly string _routingKey;
    private readonly string _exchangeName;
    protected ConsumerDefinition(string endpoint, string routingKey, string exchangeName)
    {
        EndpointName = endpoint;
        _routingKey = routingKey;
        _exchangeName = exchangeName;
    }
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, 
        IConsumerConfigurator<TConsumer> consumerConfigurator)
    {
        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            //endpointConfigurator.EndpointName = _endpoint;
            endpointConfigurator.ConfigureConsumeTopology = false;
            rmq.Bind(_exchangeName, s =>
            {
                s.RoutingKey = _routingKey;
                s.ExchangeType = "topic";
            });
        }
    }
}
