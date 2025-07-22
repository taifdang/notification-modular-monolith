
using Google.Protobuf;

namespace Hookpay.Shared.Domain.Events;

public class MessageEnvelope
{
    public MessageEnvelope(object? message, IDictionary<string, object?>? headers = null)
    {
        Message = message;
        Headers = headers;
    }

    public object? Message { get; set; }
    public IDictionary<string, object?>? Headers { get; set; }
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