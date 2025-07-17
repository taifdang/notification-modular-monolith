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
    public  Task Consume(ConsumeContext<MessageAllContracts> context)
    {
        try
        {
            _logger.LogWarning($"[message.send]::{context.Message.body}");
            return Task.CompletedTask;

        }
        catch
        {
            _logger.LogCritical($"occure error");
            return Task.CompletedTask;
        }
    }
   
}
