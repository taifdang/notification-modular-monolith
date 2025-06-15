namespace API.Notification.Models
{
    public class MessagesPayload
    {
        public string event_type { get; set; }
        public int entity_id { get; set; }       
        public string action { get; set; } //create|update
        public int user_id { get; set; }
        public Dictionary<string, object> details { get; set; }
    }
}
