using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Models;
using Hookpay.Shared.Contracts;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Events.External;

public class ReceivedMessageHandler : IConsumer<MessageContracts>
{
    private readonly MessageDbContext _context;
    public ReceivedMessageHandler(MessageDbContext context) {  _context = context; }
    public async Task Consume(ConsumeContext<MessageContracts> request)
    {
        var inbox = InboxMessage.Create(request.Message.correlationId, request.Message.eventType,request.Message.payload);
       
        var message = new Message
        {
            mess_correlationId = inbox.correlationId,
            mess_userId = default,
            mess_title = inbox.eventType,
            mess_body = inbox.payload,
            mess_createdAt = inbox.createdAt,
        };
        _context.inboxMessage.Add(inbox);
        _context.message.Add(message);
        await _context.SaveChangesAsync();             
    }
}
