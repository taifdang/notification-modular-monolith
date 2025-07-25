using Hangfire;
using Hangfire.States;
using MediatR;

namespace Hookpay.Shared.Hangfire;

public static class HangfireMediatRExtensions
{
    public static bool EnqueueCommand<T> (this IBackgroundJobClient backgroundJobClient, T command) where T : IRequest
    {
        var taskId = backgroundJobClient.Enqueue<HangfireMediator>(x => x.Run(command));

        return string.IsNullOrWhiteSpace(taskId) ? false : true;
    }
   
    public static bool ScheduleCommand<T>(this IBackgroundJobClient backgroundJobClient, T command, int timeout) where T : IRequest
    {
        var taskId =  backgroundJobClient.Schedule<HangfireMediator>(x => x.Run(command), TimeSpan.FromSeconds(timeout));

        return string.IsNullOrWhiteSpace(taskId) ? false : true;
    }
}
[Queue("message_queue")]
public class HangfireMediator
{
    private readonly IMediator mediator;

    public HangfireMediator(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public Task Run<T> (T command) where T : IRequest
    {
        return mediator.Send(command);
    }

}
