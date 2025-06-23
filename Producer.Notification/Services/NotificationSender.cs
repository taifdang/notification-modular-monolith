using ShareCommon.DTO;


namespace Producer.Notification.Services
{
    public class NotificationSender
    {
        private readonly INotifyFactory _factory;
        private readonly ILogger<NotificationSender> _logger;
        public NotificationSender(INotifyFactory factory, ILogger<NotificationSender> logger)
        {
            _factory = factory;
            _logger = logger;
        }
        public async Task HandleAsync(MessagePayload? payload)
        {
            if (payload is null)
            {
                _logger.LogWarning("[NotificationSender]:error>>action type is null");
                return;
            }
            var strategy = _factory.GetStrategy(payload.action);
            if (strategy is null)
            {
                _logger.LogWarning("[NotificationSender]:error>>No strategy found for action: {Action}", payload.action);
                return;
            }
            await strategy.SendAsync(payload);
        }
    }
}
