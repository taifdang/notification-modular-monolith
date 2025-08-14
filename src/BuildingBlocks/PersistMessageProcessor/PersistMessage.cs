

using BuildingBlocks.Core.Model;

namespace BuildingBlocks.PersistMessageProcessor;

public class PersistMessage : IVersion
{
    public PersistMessage(
        Guid id,
        string dataType,
        string data, 
        MessageDeliveryType deliveryType)
    {
        Id = id;
        DataType = dataType;
        Data = data;
        Created = DateTime.Now;
        RetryCount = 0;
        MessageStatus = MessageStatus.InProgress;
        DeliveryType = deliveryType;
    }

    public Guid Id { get; private set; }
    public string DataType { get; private set; }
    public string Data { get; private set; }
    public DateTime Created { get; private set; }
    public int RetryCount { get; private set; }
    public MessageStatus MessageStatus { get; private set; }
    public MessageDeliveryType DeliveryType { get; private set; }
    public long Version { get; set; }

    public void ChangeState(MessageStatus status)
    {
        MessageStatus = status;
    }  
}
