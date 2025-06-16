using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Topup.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DatabaseContext _db;
        protected readonly DbSet<T> _dbSet;
        public Repository(DatabaseContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var data = await _dbSet.FindAsync(id);
            return data!;
        }
    }
}
