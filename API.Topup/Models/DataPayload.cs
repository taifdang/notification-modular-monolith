namespace API.Topup.Models
{
    public class DataPayload
    {
        public int id { get; set; }
        public string action { get; set; }// topup
        public string user_id { get; set; } //create
        public string? details { get; set; } //json topup {total_amount:10000}
    }
}
