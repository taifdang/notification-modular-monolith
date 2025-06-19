using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class Messages
    {
        [Key]
        public int mess_id { get; set; }
        public Guid mess_event_id { get; set; } = default!;
        [Column(TypeName = "varchar(20)")]
        public string mess_event_type { get; set; } = default!;//create
        [Column(TypeName = "varchar(50)")]
        public string? mess_source { get; set; }//topup_service
        public string? mess_payload { get; set; }
        public DateTime mess_created_at { get; set; } 
        public int? mess_user_id { get; set; }
        public Users? users { get; set; }
    }
}
