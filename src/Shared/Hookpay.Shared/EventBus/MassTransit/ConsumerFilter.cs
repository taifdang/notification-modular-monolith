
using Hookpay.Shared.PersistMessageProcessor;
using MassTransit;

namespace Hookpay.Shared.EventBus.MassTransit;

public class ConsumerFilter<T> : IFilter<ConsumeContext<T>>
    where T : class
{
    private readonly IPersistMessageProcessor _persistMessageProcessor;
    public ConsumerFilter(IPersistMessageProcessor persistMessageProcessor)
    {
        _persistMessageProcessor = persistMessageProcessor;
    }
    public void Probe(ProbeContext context)
    {
        
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var id = await _persistMessageProcessor.AddReceivedMessageAsync(
                new MessageEnvelope(
                    context.Message,
                    context.Headers.ToDictionary(x=>x.Key, x=>x.Value))
            );

        var message = await _persistMessageProcessor.ExistMessageAsync(id);
        if(message is null)
        {
            await next.Send(context);
            await _persistMessageProcessor.ProcessInboxAsync(id);
        }
        

    }
}
