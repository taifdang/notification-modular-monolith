namespace API.Topup.Models
{
    public class MessageEvent
    {
        public string message_id { get; set; }
        public string message_type { get; set; }
        public DateTime created_at { get; set; }
        public string? source { get; set; }
        public string? data { get; set; } //json

    }
}
