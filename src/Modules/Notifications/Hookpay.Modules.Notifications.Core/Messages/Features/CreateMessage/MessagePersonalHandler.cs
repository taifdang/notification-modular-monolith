using Hookpay.Modules.Notifications.Core.Models;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.EventBus;
using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;

public class MessagePersonalHandler
{
    private readonly ILogger<MessagePersonalHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IBusPublisher _publisher;
    public MessagePersonalHandler(ILogger<MessagePersonalHandler> logger, IMediator mediator, IBusPublisher publisher)
    {
        _logger = logger;
        _mediator = mediator;
        _publisher = publisher;
    }
    public Dictionary<int, int> LoadUserCache()
    {
        //bit [userId-AllowNotification]: 0_Active;1_Locked
        var cache_caching = new Dictionary<int, int>()
        {
            {2,1},{3,0},{4,1},{5,1}
        };
        if (cache_caching is null)
        {
            //meditR.send()
        }
        return cache_caching;
    }
    public async Task SendInQueueAsync(List<Message> listMessagePersonal)
    {
        var userStatusCache = LoadUserCache();

        var listUserIdValid = listMessagePersonal
            .Where(x => userStatusCache.TryGetValue(x.UserId, out int status) && status == 0)
            .Select(x => new MessageEvent(x.CorrelationId, x.UserId, x.Title, x.Body))
            //.Distinct()
            .ToList();
        await _publisher.SendAsync<MessagePersonalContracts>(new MessagePersonalContracts(listUserIdValid));
    }

}
