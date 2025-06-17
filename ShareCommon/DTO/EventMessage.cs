using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.DTO
{
    public class EventMessage<T>//otopup_payload
    {
        public Guid message_id { get; set; }
        public string? message_type { get; set; } //topupCreated
        public DateTime timestamp { get; set; }
        public string? source { get; set; }
        public DataPayload<T> data { get; set; } = default!;
        public string? status { get; set; }
        public string? error { get; set; }

    }
}
