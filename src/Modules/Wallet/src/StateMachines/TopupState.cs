
using MassTransit;

namespace Wallet.StateMachines;

public class TopupState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;
    public string? FaultReason { get; set; }
    public Guid? TransactionId { get; set; } = default!;
    public DateTime? Updated { get; set; }
}
