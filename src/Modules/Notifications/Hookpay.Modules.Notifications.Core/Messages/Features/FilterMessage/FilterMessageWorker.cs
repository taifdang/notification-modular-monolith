using Hangfire;
using Hookpay.Modules.Notifications.Core.Data;
using Hookpay.Modules.Notifications.Core.Messages.Enums;
using Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;
using Hookpay.Modules.Notifications.Core.Messages.Features.HangfireJobHandler;
using Hookpay.Modules.Notifications.Core.Messages.Models;
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
                var messages = _context.Message
                               .Where(x => x.IsProcessed == false)
                               .OrderBy(x => x.Id)
                               .Take(10)
                               .ToList();
                if (messages is null || messages.Count() == 0)
                {
                    _logger.LogCritical($"[woker.filter]::not found message");
                    return;
                }
                var messageId = messages.Select(x => x.Id).ToList();
             
                var listMessagePersonal = new List<Message>();
                foreach (var msg in messages)
                {
                    switch (msg.MessageType)
                    {
                        case MessageType.All:
                            //await ScheduleJob(msg);
                            var _hangfireJob = scope.ServiceProvider.GetRequiredService<IHangfireJobHandler>();
                            await _hangfireJob.ScheduleJob(msg);
                            break;
                        case MessageType.Personal:
                            listMessagePersonal.Add(msg);
                            break;
                    }
                }            
                var personalHandler = scope.ServiceProvider.GetRequiredService<MessagePersonalHandler>();
                await personalHandler.SendInQueueAsync(listMessagePersonal);

                //not change field_verison <- query in database
                //await _context.Message
                //       .Where(x => messageId.Contains(x.Id))//Id
                //       .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsProcessed, true));
                foreach (var msg in messages)
                {
                    msg.IsProcessed = true;
                }
                await _context.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[woker.filter]::error>> {ex.ToString()} _ {DateTimeOffset.Now}");
            }
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
         }
    }
}
