using ShareCommon.Enum;


namespace Producer.Notification.Services
{
    public interface INotifyFactory
    {
        INotifyStrategy? GetStrategy(PushType type);
    }
}
