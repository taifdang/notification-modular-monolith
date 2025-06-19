using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareCommon.Model
{
    public class Settings
    {
        [Key]
        public int set_id { get; set; }
        public int set_user_id { get; set; } = default!;
        public bool disable_notification { get;set; } = false;
        public Users? users { get; set; }
    }
}
