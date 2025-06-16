using ShareCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Topup.Repositories
{
    public interface IUserRepository:IRepository<Users>
    {
        Task<Users> FindUser(string username);
        Task UpdateBalance(int user_id, decimal balance);
    }
}
