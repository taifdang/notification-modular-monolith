using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.DTO
{
    public class MessagePayload2
    {
        public Guid message_id { get; set; }
        public string? message_type { get; set; } 
        public DateTime timestamp { get; set; }
        public string? source { get; set; }
        public string? data { get; set; } = default!;
        public string? status { get; set; }
        public string? error { get; set; }
    }
}
