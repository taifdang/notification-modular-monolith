using BuildingBlocks.Contracts;
using BuildingBlocks.Signalr;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Wallet.Saga;

public class TopupStateMachine : MassTransitStateMachine<TopupState>
{
    private readonly IHubContext<SignalrHub> _hub;

    public State UpdatingBalance { get; private set; }
    public State SendingNotification { get; private set; }
    public State Completed { get; private set; }
    public State Failed { get; private set; }

    public Event<TopupConfirmed> TopupConfirmed { get; private set; }
    public Event<TopupFailed> TopupFailed { get; private set; }
    public Event<BalanceUpdated> BalanceUpdated { get; private set; }
    public Event<NotificationSent> NotificationSent { get; private set; }

    public TopupStateMachine(IHubContext<SignalrHub> hub)
    {
        _hub = hub;

        InstanceState(x => x.CurrentState);

        Event(() => TopupConfirmed, x => x.CorrelateById(context => context.Message.TransactionId));
        Event(() => BalanceUpdated, x => x.CorrelateById(context => context.Message.TransactionId));
        Event(() => NotificationSent, x => x.CorrelateById(context => context.Message.TransactionId));
        Event(() => TopupFailed, x => x.CorrelateById(context => context.Message.TransactionId));

        Initially(
           When(TopupConfirmed)
               .ThenAsync(async context =>
               {
                   context.Saga.Amount = context.Message.Amount;
                   context.Saga.UpdateAt = DateTime.UtcNow;

                   await UpdateState(context.Saga, "TopupConfirm", "Successed");
                   await UpdateState(context.Saga, "BalanceUpdate", "Pending");
               })
               .TransitionTo(UpdatingBalance)
        );

        During(UpdatingBalance,
           When(BalanceUpdated)
               .ThenAsync(async context =>
               {
                   context.Saga.UserId = context.Message.UserId;

                   await UpdateState(context.Saga, "BalanceUpdate", "Successed");
                   await UpdateState(context.Saga, "NotificationSend", "Pending");
               })
               .TransitionTo(SendingNotification),

           When(TopupFailed)
               .ThenAsync(async context =>
               {
                   await UpdateState(context.Saga, "BalanceUpdate", "Failed");
               })
               .TransitionTo(Failed)
               .Finalize()
        );

        During(SendingNotification,
           When(NotificationSent)
               .ThenAsync(async context =>
               {
                   await UpdateState(context.Saga, "NotificationSend", "Successed");
               })
               .TransitionTo(Completed)
               .Finalize(),
           When(TopupFailed)
               .ThenAsync(async context =>
               {
                   await UpdateState(context.Saga, "NotificationSend", "Failed");
               })
               .TransitionTo(Failed)
               .Finalize()
        );
    }

    private async Task UpdateState(TopupState instance, string currentStep, string status, string error = null)
    {
        await _hub.Clients.Group(instance.CorrelationId.ToString())
                  .SendAsync("StateUpdated", currentStep, status);
    }
}
