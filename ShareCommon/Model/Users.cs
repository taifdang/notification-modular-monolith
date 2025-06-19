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
        [Key]//prefix+name
        public int user_id { get; set; }//4byte
        [Column(TypeName = "varchar(20)")]// varchar not unicode 1byte/1digit
        public string user_name { get; set; } = default!;
        [Column(TypeName = "varchar(25)")]
        public string user_password { get; set; } = default!;
        [Column(TypeName = "decimal(18,2)")]
        public decimal user_balance { get; set; } = 0;
        [Column(TypeName = "varchar(180)")]
        public string? user_email { get; set; }
        [Column(TypeName = "varchar(12)")]
        public string? user_phone { get; set; }
        public bool is_block { get; set; } = false;
        public ICollection<Messages>? messages { get; set; }
        public Settings? settings { get; set; } 
        
    }
}
