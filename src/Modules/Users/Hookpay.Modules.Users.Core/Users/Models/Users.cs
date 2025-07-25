﻿using Hookpay.Modules.Users.Core.Users.Enums;
using Hookpay.Modules.Users.Core.Users.Features.RegisterNewUser;
using Hookpay.Shared.Core.Model;

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
        public UserSetting? UserSetting { get; set; }

        public static Users Create(string username, string password, string email, string phone)
        {
            var user = new Users
            {
                Username = username,
                Password = password,
                Email = email,
                Phone = phone
            };

            user.AddDomainEvent(new UserCreatedDomainEvent(user.Id));

            return user;
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }
    }
}
