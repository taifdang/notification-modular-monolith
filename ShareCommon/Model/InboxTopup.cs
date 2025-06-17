using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class InboxTopup
    {       
        [Key]
        public int itopup_id { get; set; }
        public int? itopup_trans_id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string itopup_event_type { get; set; } = default!;//[thesieure,sepay]
        [Column(TypeName = "varchar(50)")]
        public string? itopup_source { get; set; } //url
        public string? itopup_payload { get; set; } //json chua tat ca thong tin message tu webhook
        public DateTime itopup_created_at { get; set; }
        public DateTime? itopup_updated_at { get; set; }
        public string? itopup_error { get; set; }
        //type_payload => tu type mapping data => json (data dynamic)
    }
}
