using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class Users
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; } = default!;
        public string password { get; set; } = default!;
        [Column(TypeName = "decimal(20,2)")]
        public decimal amount_coint { get; set; } = 0;
        public string? email { get; set; }
        public string? phone { get; set; }
        
    }
}
