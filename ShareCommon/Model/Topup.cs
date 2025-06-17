using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class Topup// chuan hoa du lieu
    {
         [Key]
         public int topup_id { get; set; }
         public int topup_trans_id { get; set; } = default!;//user_id co the trung nhau
         [Column(TypeName = "varchar(50)")]
         public string? topup_source { get; set; } // momo,vnpay
         [Column(TypeName = "varchar(20)")]
         public string? topup_creator { get; set; }
         [Column(TypeName = "decimal(18,2)")]
         public decimal topup_tranfer_amount { get; set; }//10.000
         public DateTime topup_created_at { get; set; }   
    }
}
