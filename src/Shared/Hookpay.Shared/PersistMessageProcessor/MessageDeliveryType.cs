namespace Hookpay.Shared.PersistMessageProcessor;

public enum MessageDeliveryType
{
    None = 0,
    Outbox = 1,
    Inbox = 2,
    Internal = 3,
}
