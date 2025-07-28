using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Messages.Models;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Utils;
using MapsterMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Hookpay.Modules.Notifications.Core.EventListener.Consumers;

public class EventMessageListener : IConsumer<MessageCreated>
{
    private readonly MessageDbContext _context;
    private readonly IMessageConvert _convert;
    private readonly ILogger<EventMessageListener> _logger;
    private readonly IMapper _mapper;
    public EventMessageListener(
        MessageDbContext context,
        IMessageConvert convert, 
        ILogger<EventMessageListener> logger,
        IMapper mapper) 
    { 
        _context = context;
        _convert = convert;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<MessageCreated> request)
    {
        try
        {
            var data = JsonSerializer.Deserialize<MessagePayload>(request.Message.payload);
            if(data == null)
            {
                throw new Exception("convert message payload fail");
            }
            
            
            //body render
            var body = _convert.MessageRender(data.EventType, data.Data);

            //title render
            var title = request.Message.eventType;

            var alert = _mapper.Map<Alert>(data!);
            alert.Title = title;
            alert.Body = body;

            //internalEvent
            var message = Message.Create(
                request.Message.correlationId,
                data.EntityId, 
                request.Message.eventType,
                title,
                body,
                JsonSerializer.Serialize(alert));
         
            _context.Message.Add(message);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Handler {nameof(EventMessageListener)} with Event = {request.Message.eventType} is saved.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Handler {nameof(EventMessageListener)} occur error = {ex.ToString()}");
        }
    }
}
    