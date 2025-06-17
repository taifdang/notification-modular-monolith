using ShareCommon.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class OutboxTopup
    {
        [Key]
        public int otopup_id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string otopup_event_type { get; set; } = default!;//create
        [Column(TypeName = "varchar(50)")]
        public string? otopup_source { get; set; }
        public string? otopup_payload { get; set; }  //format  
        public DateTime otopup_created_at { get; set; }
        public DateTime? otopup_updated_at { get; set; }
        [Column(TypeName = "tinyint")] //1byte >> 0-255
        public StatusDispatch otopup_status { get; set; } = default!;    
    }
}
