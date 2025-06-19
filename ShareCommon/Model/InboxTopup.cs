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
    public class InboxTopup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int itopup_id { get; set; }//unique
        [Column(TypeName = "varchar(20)")]
        public string itopup_event_type { get; set; } = default!;//[thesieure,sepay]
        [Column(TypeName = "varchar(50)")]
        public string? itopup_source { get; set; } //url
        public string? itopup_payload { get; set; } //json chua tat ca thong tin message tu webhook
        public DateTime itopup_created_at { get; set; }
        public DateTime? itopup_updated_at { get; set; }
        public string? itopup_error { get; set; }
        [Column(TypeName = "tinyint")]
        public MessageStatus itopup_status { get; set; } = default!;
        //type_payload => tu type mapping data => json (data dynamic)
    }
}
