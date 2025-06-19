using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Generic
{
    public class TopupDetail
    {       
        public int user_id { get; set; } = default!;
        public string? username { get; set; } 
        public decimal transfer_amount { get; set; }
    }
}
