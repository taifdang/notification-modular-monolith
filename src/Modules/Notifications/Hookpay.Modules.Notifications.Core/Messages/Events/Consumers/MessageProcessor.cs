using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Messages.Models;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Utils;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Hookpay.Modules.Notifications.Core.Messages.Events.Consumers;

public class MessageProcessor : IConsumer<MessageCreated>
{
    private readonly MessageDbContext _context;
    private readonly IMessageConvert _convert;
    private readonly ILogger<MessageProcessor> _logger;
    public MessageProcessor(MessageDbContext context,IMessageConvert convert, ILogger<MessageProcessor> logger) 
    { 
        _context = context;
        _convert = convert;
        _logger = logger;   
    }
    public async Task Consume(ConsumeContext<MessageCreated> request)
    {
        try
        {
            var inbox = InboxMessage.Create(request.Message.correlationId, request.Message.eventType, request.Message.payload);
            var data = JsonSerializer.Deserialize<MessagePayload>(request.Message.payload);
            var body = _convert.MessageRender(data.event_type, data.detail);

            var message = Message.Create(request.Message.correlationId, data.user_id, request.Message.eventType, body);

            _context.InboxMessage.Add(inbox);
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
