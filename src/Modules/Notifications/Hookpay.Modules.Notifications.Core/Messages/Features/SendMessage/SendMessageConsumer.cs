using Hookpay.Shared.Contracts;
using Hookpay.Shared.SignalR;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.SendMessage;

public class SendMessageConsumer : IConsumer<MessageEventContracts>
{
    private readonly ILogger<SendMessageConsumer> _logger;
    private readonly INotificationHubService _hub;
    public SendMessageConsumer(
        ILogger<SendMessageConsumer> logger ,
        INotificationHubService hub
        ) 
    {  
        _logger = logger;
        _hub = hub;       
    }
    public  async Task Consume(ConsumeContext<MessageEventContracts> context)
    {
        //OUTBOX/INBOX
        try
        {
            await _hub.SendAllAsync(context.Message.body);
            _logger.LogInformation(context.Message.body);

        }
        catch
        {
            _logger.LogError("error connect");
        }
        
    }
}
