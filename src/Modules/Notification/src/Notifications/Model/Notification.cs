namespace Notification.Notifications.Model;

public record Notification 
{
    public Guid requestId { get; set; }
    public string notificationType { get; set; }
    public List<string> channels { get; set; }
    public object recipient { get; set; }
    public string message {  get; set; }
    public DateTime ScheduleAt { get; set; }
    public Dictionary<string, object> metadata { get; set; }

}
