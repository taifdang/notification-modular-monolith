using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Models;
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
    public IMediator _mediator;
    public MessageDbContext _context;
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
                _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var _publisher   = scope.ServiceProvider.GetRequiredService<IBusPublisher>();
                //Filter all => userId ???
                var messages = _context.Message.Where(x=>x.IsProcessed == false).OrderBy(x=>x.Id).Take(3).ToList();             
                if (messages is null) return;
                var listMessageId = messages.Select(x => x.Id).ToList();
                var reMessages = await FilterUser(messages);
                var listFilter = messages.Where(x => reMessages.Contains(x.UserId)).ToList();
                //


                
                //           


                foreach(var filter in listFilter)
                {
                    Console.OutputEncoding = Encoding.UTF8;                  
                    Console.WriteLine(filter.Body);
                    //send message
                    //OUTBOX
                    await _publisher.SendAsync<MessageEventContracts>(
                        new MessageEventContracts(
                            filter.CorrelationId,
                            filter.Id,
                            filter.Title,
                            filter.Body)
                        );
                }
                await _context.Message
                    .Where(x => listMessageId.Contains(x.Id))
                    .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsProcessed, true));             
            }          
            catch(Exception ex)
            {
                _logger.LogError($"[woker.filter]::error>> {ex.ToString()} _ {DateTimeOffset.Now}");
            }
            //_logger.LogCritical("[woker.filter]>>> running at: {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }       
    }
    public async Task<List<int>> FilterUser(List<Message> messages)
    {
        var userIds = messages.Select(x => x.UserId).Distinct().ToList();
        var listUserFilter = await _mediator.Send(new UserFlilterContracts(userIds));
        return listUserFilter;
       
    }
    public void LoadBatchUserCache(List<Message> messages)
    {
        //Check caching => list user enable
        //if user is locked => change user_status => AllowNotification = false
        //bit [userId-0/1]: userId - [AllowNotification]
        var cache_caching = new Dictionary<int, int>()
        {
            {1,1},{2,0},{3,1},{4,1}
        };
        while (true)
        {

        }

    }
    public void MessageHandler()
    {
        var messageActive = _context.Message.Where(x => x.IsProcessed == false).OrderBy(x => x.Id).Take(10).ToList();
        foreach(var message in messageActive)
        {

        }
    }
    public Task LoadStreaming()
    {
       
        while (true)
        {
            break;
        }
        return Task.CompletedTask;
    }
}
