using MassTransit;

namespace Topup.Saga;

public class TopupState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = default!;
    // Business data
    //public bool? TopupConfirmed { get; set; }
    //public bool? BalanceUpdated { get; set; }
    //public bool? NotificationPushed { get; set; }
    //public bool? IsTopupActive {  get; set; }
    //public bool? IsBalanceActive { get; set; }
    //public bool? IsNotificationActive { get; set; }
    //public string? ErrorMessage { get; set; }
    //public long Version {  get; set; }
}
