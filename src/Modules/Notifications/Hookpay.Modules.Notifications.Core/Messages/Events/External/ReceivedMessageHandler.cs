using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Models;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Utils;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Events.External;

public class ReceivedMessageHandler : IConsumer<MessageContracts>
{
    private readonly MessageDbContext _context;
    private readonly IMessageConvert _convert;
    private readonly ILogger<ReceivedMessageHandler> _logger;
    public ReceivedMessageHandler(MessageDbContext context,IMessageConvert convert, ILogger<ReceivedMessageHandler> logger) 
    { 
        _context = context;
        _convert = convert;
        _logger = logger;   
    }
    public async Task Consume(ConsumeContext<MessageContracts> request)
    {
        try
        {
            var inbox = InboxMessage.Create(request.Message.correlationId, request.Message.eventType, request.Message.payload);
            //
            var data = JsonSerializer.Deserialize<MessagePayload>(request.Message.payload);
            var body = _convert.MessageRender(data.event_type, data.detail);

            var message = Message.Create(request.Message.correlationId, data.user_id, request.Message.eventType, body, inbox.createdAt);

            _context.inboxMessage.Add(inbox);
            _context.message.Add(message);
            await _context.SaveChangesAsync();
            _logger.LogCritical($"[consumer.notification.receive]::___+++___+++___");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[consumer.message.receive]::error>>{ex.ToString()}");
        }
    }
}
