using Hookpay.Shared.Contracts;
using Hookpay.Shared.SignalR;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.SendMessage;

public class SendPersonalMessage : IConsumer<MessagePersonalContracts>
{
    private readonly ILogger<SendPersonalMessage> _logger;
    private readonly INotificationHubService _hub;
    public SendPersonalMessage(ILogger<SendPersonalMessage> logger,INotificationHubService hub)
    {
        _logger = logger;
        _hub = hub;
    }

    public async Task Consume(ConsumeContext<MessagePersonalContracts> context)
    {
        try
        {
            await _hub.SendAllAsync(context.Message.body);
            _logger.LogWarning($"[message.send]::{context.Message.body}");
          
        }
        catch
        {
            _logger.LogCritical($"occure error");
           
        }
    }
}
