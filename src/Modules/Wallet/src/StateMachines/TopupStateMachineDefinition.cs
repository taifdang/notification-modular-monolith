using MassTransit;

namespace Wallet.StateMachines;

public class TopupStateMachineDefinition : SagaDefinition<TopupState>
{
    public TopupStateMachineDefinition()
    {
        ConcurrentMessageLimit = 12;
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<TopupState> sagaConfigurator)
    {
        
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 5000, 10000));
        //endpointConfigurator.UseInMemoryOutbox();
    }
}
