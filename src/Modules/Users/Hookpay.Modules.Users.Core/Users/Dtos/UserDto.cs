using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Dtos;

public class UserDto
{
    public int Id {  get; set; }    
    public string Username { get; set; }    
    public string Email { get;set; }
    public decimal? Balance {  get; set; }
    public string Phone { get; set; }
}
