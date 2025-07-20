using Hangfire;
using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage.CreateMessageAll;
using Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage.CreateMessagePersonal;
using Hookpay.Modules.Notifications.Core.Messages.Models;
using Hookpay.Shared.Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Hookpay.Modules.Notifications.Core.Messages.Background;

public class PersistMessageProcessor : IPersistMessageProcessor
{
    private readonly MessageDbContext _messageDbContext;
    private readonly IBackgroundJobClient _backgroundJob;
    public PersistMessageProcessor(MessageDbContext messageDbContext, IBackgroundJobClient backgroundJob)
    {
        _messageDbContext = messageDbContext;
        _backgroundJob = backgroundJob;
    }

    public async Task ChangeMessageStatusAsync(Message message, CancellationToken cancellationToken = default)
    {
        message.ChangeState(true);
        
        _messageDbContext.Message.Update(message);

        await _messageDbContext.SaveChangesAsync();      
    }

    public async Task ProcessAllAsync(CancellationToken cancellationToken = default)
    {
        var messages = _messageDbContext.Message.Where(x => x.IsProcessed == false).ToList();

        foreach(var message in messages)
        {
            await ProcessAsync(message.Id, message.MessageType, cancellationToken);
        }
    }

    public async Task ProcessAsync(int messsageId, MessageType messageType, CancellationToken cancellationToken = default)
    {
        var message = await _messageDbContext.Message.FirstOrDefaultAsync(x => x.Id == messsageId && x.MessageType == messageType, cancellationToken);

        if (message == null)
            return;
        
        switch(messageType)
        {
            case MessageType.All:

                var sendAll = _backgroundJob.ScheduleCommand(new CreateMessageAll(), 30);

                if (sendAll)
                {
                    await ChangeMessageStatusAsync(message, cancellationToken);
                    break;
                }
                else
                {
                    return;
                }
            case MessageType.Personal:

                var sendPersonal = _backgroundJob.EnqueueCommand(new CreateMessagePersonal());

                if (sendPersonal)
                {
                    await ChangeMessageStatusAsync(message, cancellationToken);
                    break;
                }
                else
                {
                    return;
                }
        }
    }
}
