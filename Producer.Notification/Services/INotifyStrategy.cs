using ShareCommon.DTO;
using ShareCommon.Enum;


namespace Producer.Notification.Services
{
    public interface INotifyStrategy
    {
        PushType channel { get; }
        Task SendAsync(MessagePayload? payload);
    }
}
