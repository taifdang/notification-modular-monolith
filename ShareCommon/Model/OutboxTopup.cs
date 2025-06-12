using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class OutboxTopup
    {
        [Key]
        public int id { get; set; }
        public string message_type { get; set; } = default!;//create
        public string? source { get; set; }
        public string? payload { get; set; }  //format  
        public DateTime created_at { get; set; }
        public DateTime? processed_at { get; set; }
        public string? status { get; set; }      
    }
}
