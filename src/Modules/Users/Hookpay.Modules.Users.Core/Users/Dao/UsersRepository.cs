using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.Users.Events;
using Hookpay.Modules.Users.Core.Users.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Dao;

public class UsersRepository : IUserRepository
{
    private readonly UserDbContext _context;
    private readonly DbSet<User> _users;
    public UsersRepository(UserDbContext context)
    {
        _context = context;
        _users = _context.users;
    }

    public async Task AddAsync(User command)
    {      
        await _users.AddAsync(command);
        await _context.SaveChangesAsync();
    }
    public async Task<User> GetAsync(int userId)
    {
        return await _users.SingleOrDefaultAsync(x => x.user_id == userId);
    }

}
