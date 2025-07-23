
using Google.Protobuf;

namespace Hookpay.Shared.PersistMessageProcessor;

public class MessageEnvelope
{
    public MessageEnvelope(object? message, IDictionary<string, object?>? headers = null)
    {
        Message = message;
        Headers = headers ?? new Dictionary<string, object?>();
    }

    public object? Message { get; init; }
    public IDictionary<string, object?> Headers { get; init; }
}

public class MessageEnvelope<T> : MessageEnvelope
    where T : class, IMessage
{
    public MessageEnvelope(T message, IDictionary<string, object?> header) : base(message, header)
    {
        Message = message;
    }

    public new T? Message { get; }
}