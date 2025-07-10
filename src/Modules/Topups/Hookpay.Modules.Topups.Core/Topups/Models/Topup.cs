using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core.Topups.Models
{
    public class Topup
    {
        //[Key]
        public int topup_id { get; set; }
        public int topup_trans_id { get; set; } = default!;
        //[Column(TypeName = "varchar(50)")]
        public string? topup_source { get; set; }
        //[Column(TypeName = "varchar(20)")]
        public string? topup_creator { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        public decimal topup_tranfer_amount { get; set; }
        public DateTime topup_created_at { get; set; }
    }
}
