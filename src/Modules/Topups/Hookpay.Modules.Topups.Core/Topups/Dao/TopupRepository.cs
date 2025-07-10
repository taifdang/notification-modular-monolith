using Hookpay.Modules.Topups.Core.Data;
using Hookpay.Modules.Topups.Core.Topups.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Core.Topups.Dao
{
    public class TopupRepository : ITopupRepository
    {
        private readonly TopupDbContext _db;
        private readonly DbSet<Topup> _topup;
        public TopupRepository(TopupDbContext db)
        {
            _db = db;
            _topup = _db.topup;       
        }

        public async Task AddAsync(Topup topup)
        {
            try
            {
                await _topup.AddAsync(topup);
                await _db.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {              
                Console.WriteLine(ex.ToString());
            }
        }
        
    }
}
