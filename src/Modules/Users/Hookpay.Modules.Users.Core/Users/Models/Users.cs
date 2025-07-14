using Hookpay.Shared.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Models
{
    public class Users:Aggregate
    {      
        public int user_id { get; set; }
        public string user_name { get; set; } 
        public string user_password { get; set; }
        public decimal user_balance { get; set; }
        public string? user_email { get; set; }
        public string? user_phone { get; set; }
        public bool is_block { get; set; } = false;     
        public UserSetting? settings { get; set; }
    }
}
