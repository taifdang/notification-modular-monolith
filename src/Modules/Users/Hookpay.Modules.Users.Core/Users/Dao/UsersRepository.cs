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
    private readonly DbSet<Models.Users> _users;
    public UsersRepository(UserDbContext context)
    {
        _context = context;
        _users = _context.Users;
    }

    public async Task AddAsync(Models.Users command)
    {      
        await _users.AddAsync(command);
        await _context.SaveChangesAsync();
    }
    public async Task<Models.Users> GetAsync(int userId)
    {
        return await _users.SingleOrDefaultAsync(x => x.Id == userId);
    }

}
