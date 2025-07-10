using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Models
{
    public class UserSetting
    {
        public int set_id { get; set; }
        public int set_user_id { get; set; }
        public bool disable_notification { get; set; }
        public User? users { get; set; }
    }
}
