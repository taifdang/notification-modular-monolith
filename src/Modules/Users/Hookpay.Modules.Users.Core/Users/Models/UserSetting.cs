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
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool AllowNotification { get; set; }
        public Users? Users { get; set; }
    }
}
