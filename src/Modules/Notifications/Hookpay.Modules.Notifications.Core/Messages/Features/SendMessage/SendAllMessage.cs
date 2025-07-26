using Hookpay.Shared.Contracts;
using Hookpay.Shared.SignalR;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.SendMessage;

public class SendAllMessage : IConsumer<MessageAllContracts>
{
    private readonly ILogger<SendAllMessage> _logger;
    private readonly INotificationHubService _hub;
    public SendAllMessage(
       ILogger<SendAllMessage> logger,
       INotificationHubService hub
       )
    {
        _logger = logger;
        _hub = hub;
    }
    public  async Task Consume(ConsumeContext<MessageAllContracts> context)
    {
        try
        {
            //await _hub.SendAllAsync(context.Message.body);
            await _hub.SendPersonalAsync(context.Message.userId.ToString(),context.Message.body);
            _logger.LogWarning($"[message.send]::{context.Message.body}");
            
        }
        catch
        {
            _logger.LogCritical($"occure error");
           
        }
    }
   
}
