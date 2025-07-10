using Hookpay.Modules.Users.Core.Users.Events;
using Hookpay.Modules.Users.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Dao;

public interface IUserRepository
{
    Task<User> GetAsync(int userId);
    Task AddAsync(User command);
}
