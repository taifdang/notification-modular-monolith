

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

    public Guid Id { get; set; }
    public string DataType { get; set; }    
    public string Data { get; set; }
    public DateTime Created {  get; set; }
    public int RetryCount { get; set; } 
    public MessageStatus MessageStatus { get; set; }
    public MessageDeliveryType DeliveryType { get; set; }
    public long Version { get; set; }

    public void ChangeState(MessageStatus status)
    {
        MessageStatus = status;
    }  
}
