using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Models;

public class UserInfo
{
    public string username { get; set; }
    public string password { get; set; }    
    public string email { get; set; }
    public string phone { get; set; }
}
