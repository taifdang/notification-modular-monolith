using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class Messages
    {
        [Key]
        public int id { get; set; }
        public string? message_id { get; set; }
        public string event_type { get; set; } = default!;//create
        public string? source { get; set; }//topup_service
        public string? payload { get; set; }
        public DateTime created_at { get; set; }        
    }
}
