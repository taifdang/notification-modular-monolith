

namespace BuildingBlocks.PersistMessageProcessor;

public class MessageEnvelope
{
    public MessageEnvelope(object? message, IDictionary<string,object?> headers = null) 
    {
        Message = message;
        Headers = headers ?? new Dictionary<string,object?>();
    }

    public object? Message { get; set; }
    public IDictionary<string, object?> Headers { get; set; } 
}

public class MessageEnvelope<T> : MessageEnvelope
{
    public MessageEnvelope(T message, IDictionary<string, object?> headers) : base(message, headers)
    {
        Message = message;
    }
    public new T? Message { get; set; }
}
