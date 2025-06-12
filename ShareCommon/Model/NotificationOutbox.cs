using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class NotificationOutbox
    {
        public int message_id { get; set; }
        public string message_type { get; set; } = default!;//notification_payment|notification_order|notification_sys
        public string? source { get; set; }//order_service|payment_service
        public string payload { get; set; } = default!;//messages
        public DateTime created_at { get; set; }
        public DateTime? processed_at { get; set; }
        public string? status { get; set; }
    }
}
