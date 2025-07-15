using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Models;
using Hookpay.Shared.Caching;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.EventBus;
using MassTransit.Util;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.FilterMessage;

public class FilterMessageWorker : BackgroundService
{
    private readonly IServiceScopeFactory _prodvider;
    private readonly ILogger<FilterMessageWorker> _logger;
    public IMediator _mediatr;
    public MessageDbContext _context;
    public IBusPublisher _publisher;
    public FilterMessageWorker(ILogger<FilterMessageWorker> logger, IServiceScopeFactory provider)
    {
        _logger = logger;
        _prodvider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //filter message push vao queue
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var scope = _prodvider.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<MessageDbContext>();
                _mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
                _publisher = scope.ServiceProvider.GetRequiredService<IBusPublisher>();

                #region command
                //    //Filter all => userId ???
                //    var messages = _context.Message.Where(x=>x.IsProcessed == false).OrderBy(x=>x.Id).Take(3).ToList();             
                //    if (messages is null) return;
                //    var listMessageId = messages.Select(x => x.Id).ToList();
                //    var reMessages = await FilterUser(messages);
                //    var listFilter = messages.Where(x => reMessages.Contains(x.UserId)).ToList();                            
                //    //           

                //    foreach(var filter in listFilter)
                //    {
                //        Console.OutputEncoding = Encoding.UTF8;                  
                //        Console.WriteLine(filter.Body);
                //        //send message
                //        //OUTBOX
                //        await _publisher.SendAsync<MessageEventContracts>(
                //            new MessageEventContracts(
                //                filter.CorrelationId,
                //                filter.Id,
                //                filter.Title,
                //                filter.Body)
                //            );
                //    }
                //    await _context.Message
                //        .Where(x => listMessageId.Contains(x.Id))
                //        .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsProcessed, true));
                #endregion
                var messages = _context.Message
                               .Where(x => x.IsProcessed == false)
                               .OrderBy(x => x.Id)
                               .Take(10)
                               .ToList();
                if (messages is null)
                {
                    return;
                }
                var listMessagePersonal = new List<Message>();
                foreach (var msg in messages)
                {
                    switch (msg.MessageType)
                    {
                        case MessageType.All:
                            //handler
                            await MessageAllHandler(msg);
                            break;
                        case MessageType.Personal:
                            listMessagePersonal.Add(msg);
                            break;
                    }
                }
                await MessagePersonalHandler(listMessagePersonal);

                await _context.Message
                       .Where(x => messages.Contains(x))//Id
                       .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsProcessed, true));
            }
            catch (Exception ex)
            {
                _logger.LogError($"[woker.filter]::error>> {ex.ToString()} _ {DateTimeOffset.Now}");
            }
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }       
    }
    public async Task<List<int>> FilterUser(List<Message> messages)
    {
        var userIds = messages.Select(x => x.UserId).Distinct().ToList();
        var listUserFilter = await _mediatr.Send(new UserFlilterContracts(userIds));
        return listUserFilter;
       
    }
    public Dictionary<int,int> LoadBatchUserCache()
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
    public async Task MessagePersonalHandler(List<Message> listMessagePersonal)
    {
        var userStatusCache = LoadBatchUserCache();
       
        var listUserIdValid = listMessagePersonal
            .Where(x => userStatusCache.TryGetValue(x.UserId, out int status) && status == 0)
            .Select(x => new MessageEvent(x.CorrelationId, x.UserId, x.Title, x.Body))
            //.Distinct()
            .ToList();
        await _publisher.SendAsync<MessagePersonalContracts>(new MessagePersonalContracts(listUserIdValid));
    }
    public async Task MessageAllHandler(Message message)
    {
        await _publisher.SendAsync<MessageAllContracts>(
            new MessageAllContracts(message.CorrelationId,message.Id,message.Title,message.Body));
    }  
}
