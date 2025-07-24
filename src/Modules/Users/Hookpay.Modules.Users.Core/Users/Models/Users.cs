using Hookpay.Modules.Users.Core.Users.Enums;
using Hookpay.Shared.Domain.Models;

namespace Hookpay.Modules.Users.Core.Users.Models
{
    public class Users:Aggregate
    {      
        public int Id { get; set; }
        public string Username { get; set; } 
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public UserStatus Status { get; set; }     
        // ? 
        public UserSetting? UserSetting { get; set; }
    }
}
