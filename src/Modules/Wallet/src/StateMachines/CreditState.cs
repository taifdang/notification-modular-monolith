using MassTransit;

namespace Wallet.Saga;

public class CreditState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;

    //Business data
    public decimal Amount { get; set; }
    public Guid? UserId { get; set; }
    public DateTime? UpdateAt { get; set; }
    public string? ErrorMessage { get; set; }
}
