using BuildingBlocks.Contracts;
using MassTransit;

namespace Topup.Saga;

public class TopupStateMachine : MassTransitStateMachine<TopupState>
{
    public State ProcessingBalance { get; set; }
    public State Notifying {  get; set; }
    public State Completed { get; private set; }
    public State Failed { get; private set; }

    public Event<TopupCreated> TopupCreated {  get; private set; }
    public Event<BalanceUpdated> BalanceUpdated { get; private set; }
    public Event<PersonalNotificationRequested> NotificationPushed { get; private set; }

    public TopupStateMachine()
    {
        InstanceState(x => x.CurrentState);

        //Event(() => TopupCreated, x => x.CorrelateBy(context => context.Message.CorrelationId));
        //Event(() => TopupCreated);
        //Event(() => BalanceUpdated);
        //Event(() => NotificationPushed, x => x.CorrelateById(context => context.Message.RequestId));

        //Initially(
        //    When(TopupCreated)
        //        .Then(context => Console.WriteLine("Saga: TopupCreated"))
        //        .TransitionTo(TopupStep));

        //During(TopupStep,
        //    When(BalanceUpdated)
        //        .Then(context => Console.WriteLine("Saga: BalanceUpdated"))
        //        .TransitionTo(BalanceStep));

        //During(BalanceStep,
        //   When(NotificationPushed)
        //       .Then(context => Console.WriteLine("Saga: NotificationPushed"))
        //       .TransitionTo(NotificationStep));


    }
}
