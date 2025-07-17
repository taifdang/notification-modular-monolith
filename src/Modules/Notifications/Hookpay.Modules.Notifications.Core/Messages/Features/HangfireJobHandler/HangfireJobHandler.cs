using Hangfire;
using Hookpay.Modules.Notifications.Core.Messages.Features.CreateMessage;
using Hookpay.Modules.Notifications.Core.Models;
using Hookpay.Shared.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Notifications.Core.Messages.Features.HangfireJobHandler;

public interface IHangfireJobHandler
{
    Task ScheduleJob(Message message);
}
public class HangfireJobHandler:IHangfireJobHandler
{
    private readonly IServiceScopeFactory _provider;
    private readonly ILogger<HangfireJobHandler> _logger;
    private readonly IBackgroundJobClient _backgroundJob;
    public HangfireJobHandler(IServiceScopeFactory provider,ILogger<HangfireJobHandler> logger, IBackgroundJobClient backgroundJob)
    {
        _provider = provider;
        _logger = logger;
        _backgroundJob = backgroundJob;
    }

    public Task ScheduleJob(Message message)
    {
        //using var scope = _provider.CreateScope();
        //var messageHandler = scope.ServiceProvider.GetRequiredService<MessageAllHandler>();
        //Hangfire.BackgroundJob.Schedule(() => messageHandler.LoadDataStreaming(message), TimeSpan.FromMinutes(5));
        _backgroundJob.Schedule<MessageAllHandler>(x => x.LoadDataStreaming(message), TimeSpan.FromSeconds(10));
        return Task.CompletedTask;  
    }
}
