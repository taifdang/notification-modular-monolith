using ShareCommon.Enum;


namespace Producer.Notification.Services
{
    public class NotifyFactory : INotifyFactory
    {
        private readonly IEnumerable<INotifyStrategy> _strategy;
        public NotifyFactory(IEnumerable<INotifyStrategy> strategy) { _strategy = strategy; }   
        public INotifyStrategy? GetStrategy(PushType type)
        {
            return _strategy.FirstOrDefault(x=>x.channel == type) ?? null;
        }
    }
}
