
using BuildingBlocks.Configuration;
using BuildingBlocks.EFCore;
using Hangfire;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Hangfire;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfireStorageMssql(this IServiceCollection services)
    {
        var options = services.GetOptions<MssqlOptions>("mssql");

        services.AddHangfire(x => x.UseSqlServerStorage(options.ConnectionString));

        return services;
    }

    public static bool EnqueueCommand<TRequest> (
        this IBackgroundJobClient backgroundJobClient, 
        TRequest command)
        where TRequest : IRequest
    {
        var runTask = backgroundJobClient.Enqueue<HangfireMediator>(x => x.RunTaskAsync(command));

        return string.IsNullOrWhiteSpace(runTask) ? false : true;
    }

    public static bool SheduleCommand<TRequest>(
        this IBackgroundJobClient backgroundJobClient,
        TRequest command,
        int timeout)
        where TRequest : IRequest

    {
        var runTask = backgroundJobClient.Schedule<HangfireMediator>(x => x.RunTaskAsync(command), TimeSpan.FromSeconds(timeout));

        return string.IsNullOrWhiteSpace(runTask) ? false : true;
    }


    [Queue("message_queue")]
    public class HangfireMediator
    {
        private readonly IMediator _mediator;

        public HangfireMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task RunTaskAsync<T>(T command) where T : IRequest
        {
            return _mediator.Send(command);
        }
    }
}
