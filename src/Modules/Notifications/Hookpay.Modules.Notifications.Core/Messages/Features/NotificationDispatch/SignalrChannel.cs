using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Features.SendMessage;
using Hookpay.Modules.Notifications.Core.Messages.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.NotificationDispatch;

public class SignalrChannel : INotificationChannel
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly ILogger<SignalrChannel> _logger;   

    public PushType PushType => PushType.InApp;

    public SignalrChannel(
        ISendEndpointProvider sendEndpointProvider,
        ILogger<SignalrChannel> logger)
    {
        _sendEndpointProvider = sendEndpointProvider;
        _logger = logger;   
    }

    public async Task SendAsync(int target, Alert alert)
    {
        try
        {
            var endpoint = await _sendEndpointProvider
            .GetSendEndpoint(new Uri("queue:signalr-channel-queue-handler"));

            //convert?

            await endpoint.Send(new SignalrChannelQueue(target, alert));

            _logger.LogInformation("Send in queue via signalr channel is processed");
        }
        catch(Exception ex)
        {
            _logger.LogInformation("Send in queue via signalr channel is error, {ex}", ex);
        }
    }
}
