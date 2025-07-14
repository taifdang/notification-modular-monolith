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
                var _context = scope.ServiceProvider.GetRequiredService<MessageDbContext>();
                _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var _publisher   = scope.ServiceProvider.GetRequiredService<IBusPublisher>();

                var messages = _context.Message.Where(x=>x.mess_processed == false).OrderBy(x=>x.mess_id).Take(3).ToList();             
                if (messages is null) return;
                var listMessageId = messages.Select(x => x.mess_id).ToList();
                var reMessages = await FilterUser(messages);
                var listFilter = messages.Where(x => reMessages.Contains(x.mess_userId)).ToList();
                             
                foreach(var filter in listFilter)
                {
                    Console.OutputEncoding = Encoding.UTF8;                  
                    Console.WriteLine(filter.mess_body);
                    //send message
                    //OUTBOX
                    await _publisher.SendAsync<MessageEventContracts>(
                        new MessageEventContracts(
                            filter.mess_correlationId,
                            filter.mess_id,
                            filter.mess_title,
                            filter.mess_body)
                        );
                }
                await _context.Message
                    .Where(x => listMessageId.Contains(x.mess_id))
                    .ExecuteUpdateAsync(x => x.SetProperty(y => y.mess_processed, true));             
            }          
            catch(Exception ex)
            {
                _logger.LogError($"[woker.filter]::error>> {ex.ToString()}");
            }
            _logger.LogCritical("[woker.filter]>>> running at: {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }       
    }
    public async Task<List<int>> FilterUser(List<Message> messages)
    {
        var userIds = messages.Select(x => x.mess_userId).Distinct().ToList();
        var listUserFilter = await _mediator.Send(new UserFlilterContracts(userIds));
        return listUserFilter;
       
    }
}
