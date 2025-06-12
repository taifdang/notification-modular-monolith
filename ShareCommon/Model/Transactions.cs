using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class Transactions// chuan hoa du lieu
    {
         [Key]
         public int id { get; set; }
         public int transaction_id { get; set; } = default!;//id co the trung nhau
         public string? source { get; set; } // momo,vnpay
         public string? username { get; set; }
         [Column(TypeName = "decimal(20,2)")]
         public decimal tranfer_amount { get; set; }//10.000
         public DateTime create_at { get; set; }   
    }
}
