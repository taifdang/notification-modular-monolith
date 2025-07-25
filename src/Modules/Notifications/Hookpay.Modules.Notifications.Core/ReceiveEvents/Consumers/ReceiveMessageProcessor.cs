using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Messages.Models;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Utils;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Hookpay.Modules.Notifications.Core.ReceiveEvents.Consumers;

public class ReceiveMessageProcessor : IConsumer<MessageCreated>
{
    private readonly MessageDbContext _context;
    private readonly IMessageConvert _convert;
    private readonly ILogger<ReceiveMessageProcessor> _logger;
    public ReceiveMessageProcessor(MessageDbContext context,IMessageConvert convert, ILogger<ReceiveMessageProcessor> logger) 
    { 
        _context = context;
        _convert = convert;
        _logger = logger;   
    }
    public async Task Consume(ConsumeContext<MessageCreated> request)
    {
        try
        {
            //var inbox = InboxMessage.Create(request.Message.correlationId, request.Message.eventType, request.Message.payload);
            var data = JsonSerializer.Deserialize<MessagePayload>(request.Message.payload);

            var body = _convert.MessageRender(data.EventType, data.MetaData);

            //internal event => send messsage
            var message = Message.Create(
                request.Message.correlationId,
                data.UserId, 
                request.Message.eventType,
                body,
                JsonSerializer.Serialize(data));//?
         

            _context.Message.Add(message);
            await _context.SaveChangesAsync();

            _logger.LogCritical($"[consumer.notification.receive]::{DateTime.Now}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[consumer.message.receive]::error::{ex.ToString()}");
        }
    }
}
