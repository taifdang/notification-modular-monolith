using Hookpay.Modules.Notifications.Core.Messages.Models;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.EventBus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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
    private readonly IServiceScopeFactory _provider;
    private readonly IMediator _mediator;
    public MessagePersonalHandler(ILogger<MessagePersonalHandler> logger, IMediator mediator, IServiceScopeFactory provider)
    {
        _logger = logger;
        _mediator = mediator;
        _provider = provider;
    }
    public Dictionary<int, int> LoadUserCache()
    {
        //bit [userId-AllowNotification]: 0_Active;1_Locked
        var cache_caching = new Dictionary<int, int>()
        {
            {2,0},{3,0},{4,0},{5,0}
        };
        if (cache_caching is null)
        {
            //meditR.send()
        }
        return cache_caching;
    }
    public async Task SendInQueueAsync(List<Message> listMessagePersonal)
    {
        try
        {
            var userStatusCache = LoadUserCache();

            using var scope = _provider.CreateScope();
            var _publisher = scope.ServiceProvider.GetRequiredService<IBusPublisher>();

            var listUserIdValid = listMessagePersonal
                .Where(x => userStatusCache.TryGetValue(x.UserId, out int status) && status == 0)
                .Select(x => new MessagePersonalContracts(x.CorrelationId, x.UserId, x.Title, x.Body))
                //.Distinct()
                .ToList();
            //await _publisher.SendAsync<MessagePersonalContracts>(new MessagePersonalContracts(listUserIdValid));
            foreach (var item in listUserIdValid)
            {
                await _publisher.SendAsync<MessagePersonalContracts>(item);
            }
        }
        catch(Exception ex) 
        {
            _logger.LogError($"[messeage.personal]::{ex.ToString()}");
        }
    }

}
