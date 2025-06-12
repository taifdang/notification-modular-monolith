using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class InboxTopup
    {
        //{1,"sepay","sepay_url","data...","Datetime.Now"}
        [Key]
        public int topup_id { get; set; }
        public string topup_type { get; set; } = default!;//[thesieure,sepay]
        public string? source { get; set; } //url
        public string? payload { get; set; } //json chua tat ca thong tin message tu webhook
        public DateTime create_at { get; set; }
        public DateTime? process_at { get; set; }
        public string? error { get; set; }
        //type_payload => tu type mapping data => json (data dynamic)
    }
}
