

using Hookpay.Shared.Domain.Events;
using System.Text.Json;

namespace Hookpay.Shared.PersistMessageProcessor;

public class PersistMessageProcessor : IPersistMessageProcessor
{
    public Task PublishMessageAsync<T>(T messageEnvelope, CancellationToken cancellationToken = default) 
        where T : MessageEnvelope
    {
        if (messageEnvelope is null)
            throw new Exception("messageEnvelope is empty or null");

        var data = JsonSerializer.Serialize(messageEnvelope);

        Console.WriteLine($"[{messageEnvelope.GetType().ToString()}]:::{data}");
        return Task.CompletedTask;
    }

    public Task AddInternalMessageAsync<T>(T internalCommand, CancellationToken cancellationToken)
        where T : class, IInternalCommand
    {
        throw new NotImplementedException();
    }
}
