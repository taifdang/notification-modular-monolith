using ShareCommon.DTO;
using ShareCommon.Enum;


namespace Producer.Notification.Services
{
    public class EmailStrategy : INotifyStrategy
    {
        public PushType channel => PushType.Email;

        public Task SendAsync(MessagePayload? payload)
        {
            throw new NotImplementedException();
        }
    }
}
