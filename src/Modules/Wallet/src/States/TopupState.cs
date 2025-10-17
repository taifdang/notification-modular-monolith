
using MassTransit;

namespace Wallet.States;

public class TopupState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    //State
    public string CurrentState { get; set; } = default!;
    public string? ErrorStep { get; set; }
    public string? ErrorMessage { get; set; }

    //Business data
    public decimal Amount { get; set; }
    public Guid? UserId { get; set; }
}
