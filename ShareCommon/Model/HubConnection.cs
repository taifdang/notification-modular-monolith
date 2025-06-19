using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class HubConnection
    {
        [Key]
        public int hub_id { get; set; }
        public string? hub_connection_id { get; set; }
        public string? hub_user_name { get; set; }
    }
}
