using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;
using ShareCommon.Model;

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

        public  Task UpdateStatus(InboxTopup data)
        {
            //_db.Entry<InboxTopup>(data).CurrentValues.SetValues(data.itopup_updated_at=DateTime.Now);
            _db.Entry(data).Property(u => u.itopup_updated_at).CurrentValue = DateTime.Now; 
            return Task.CompletedTask;
        }
    }
}
