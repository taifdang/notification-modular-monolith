using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Dtos;

public class UserDto
{
    public int id {  get; set; }    
    public string username { get; set; }    
    public string email { get;set; }
    public decimal? balance {  get; set; }
    public string phone { get; set; }
}
