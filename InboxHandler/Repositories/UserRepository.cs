using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;
using ShareCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Topup.Repositories
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        public UserRepository(DatabaseContext db) : base(db)
        {
        }

        public async Task<Users> FindUser(string username)
        {
            var data = await _db.users.FirstOrDefaultAsync(x=>x.user_name == username);
            return data!;
        }

        public async Task UpdateBalance(int user_id, decimal balance)
        {          
            await _db.users
                .Where(u => u.user_id == user_id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.user_balance, u => u.user_balance + balance));
        }
    }
}
