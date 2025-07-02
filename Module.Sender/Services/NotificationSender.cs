using ShareCommon.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Sender.Services
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
        public async Task HandleAsync(NotifyPayload? payload)
        {
            if (payload is null)
            {
                _logger.LogWarning("[NotificationSender]:error>>action type is null");
                return;
            }
            var strategy = _factory.GetStrategy(payload.push_type);
            if (strategy is null)
            {
                _logger.LogWarning("[NotificationSender]:error>>No strategy found for action: {Action}", payload.push_type);
                return;
            }
            await strategy.SendAsync(payload);
        }
    }
}
