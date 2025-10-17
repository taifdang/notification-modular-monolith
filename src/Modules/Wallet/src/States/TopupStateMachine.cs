
using BuildingBlocks.Contracts;
using MassTransit;

namespace Wallet.States;

public class TopupStateMachine : MassTransitStateMachine<TopupState>
{
    public TopupStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => TopupConfirmed, x => x.CorrelateById(context => context.Message.TransactionId));
        Event(() => TopupFailed, x => x.CorrelateById(context => context.Message.TransactionId));
        Event(() => BalanceUpdated, x => x.CorrelateById(context => context.Message.TransactionId));
        Event(() => BalanceUpdateFailed, x => x.CorrelateById(context => context.Message.TransactionId));
        Event(() => NotificationSent, x => x.CorrelateById(context => context.Message.TransactionId));
        Event(() => NotificationFailed, x => x.CorrelateById(context => context.Message.TransactionId));

        Initially(
            When(TopupConfirmed)
              .Then(ctx =>
              {
                  ctx.Saga.Amount = ctx.Message.Amount;
              })
              .TransitionTo(TopupProcessing)
        );

        During(TopupProcessing,
            When(BalanceUpdated)
              .Then(ctx =>
              {
                 ctx.Saga.UserId = ctx.Message.UserId;
              })
              .TransitionTo(BalanceUpdating),

           When(BalanceUpdateFailed)
              .Then(ctx =>
              {
                    
              })
              .TransitionTo(Failed)
              .Finalize(),

           When(TopupFailed)
              .Then(ctx =>
              {

              })
              .TransitionTo(Failed)
              .Finalize()
        );

        During(BalanceUpdating,
            When(NotificationSent)
              .TransitionTo(NotificationProcessing),
            When(NotificationFailed)
              .Then(ctx =>
              {

              })
              .TransitionTo(Failed)
              .Finalize()
        );

        SetCompletedWhenFinalized();
    }

    public State TopupProcessing { get; private set; }
    public State BalanceUpdating { get; private set; }
    public State NotificationProcessing { get; private set; }
    public State Completed { get; private set; }
    public State Failed { get; private set; }

    public Event<TopupConfirmedEvent> TopupConfirmed { get; private set; }
    public Event<BalanceUpdatedEvent> BalanceUpdated { get; private set; }
    public Event<NotificationSentEvent> NotificationSent { get; private set; }
    public Event<TopupFailedEvent> TopupFailed { get; private set; }
    public Event<BalanceUpdateFailedEvent> BalanceUpdateFailed { get; private set; }
    public Event<NotificationFailedEvent> NotificationFailed { get; private set; }
    

}
