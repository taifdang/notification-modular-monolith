using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class InboxNotification
    {
        [Key]
        public int id { get; set; }
        public Guid message_id { get; set; }
        public string event_type { get; set; } = default!;//[thesieure,sepay]
        public string? source { get; set; } //url
        public string? payload { get; set; } //json chua tat ca thong tin message tu webhook
        public DateTime created_at { get; set; }
        public DateTime? processed_at { get; set; }
        public int retry { get; set; } = 0;
        public string? status { get; set; }
    }
}
