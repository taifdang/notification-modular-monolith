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
    public class InboxRepository : Repository<InboxTopup>, IInboxRepository
    {
        public InboxRepository(DatabaseContext db) : base(db)
        {
        }

        public async Task<List<InboxTopup>> GetListInbox()
        {
            var data = await _db.inbox_topup
                .Where(x => x.itopup_updated_at == null)
                .Take(10).ToListAsync();
            return data;
        }
    }
}
