using Hookpay.Modules.Notifications.Core.Models;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.EventBus;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;

public class MessageAllHandler
{
    private readonly IBusPublisher _publisher;
    private readonly ILogger<MessageAllHandler> _logger;
    private readonly IMediator _mediator;
    public MessageAllHandler(IBusPublisher publisher, ILogger<MessageAllHandler> logger, IMediator mediator)
    {
        _publisher = publisher;
        _logger = logger;
        _mediator = mediator;
    }
    public async Task LoadDataStreaming(Message message)
    {
        try
        {
            int totalUser = await _mediator.Send(new UserTotalItem());
            int pageSize = 10;
            int pageCount = (int)Math.Ceiling(totalUser / (decimal)pageSize);
            int pageIndex = 0;

            while (true)
            {
                var listUser = await _mediator.Send(new GetUserByPageIndex(pageIndex, pageSize));
                if (listUser == null) break;
                await SendInQueueAsync(listUser, message);
                pageIndex++;
            }
        }
        catch
        {
            throw new Exception("Occur error in hangfire caller");
        }
    }   
    public async Task SendInQueueAsync(List<int> UserIds, Message message)
    {
        try
        {
            foreach (var id in UserIds)
            {
                await _publisher.SendAsync<MessageAllContracts>(new MessageAllContracts(message.CorrelationId, id, message.Title, message.Body));
            }
        }
        catch
        {
            throw new Exception("Occur error");
        }
    }
}
