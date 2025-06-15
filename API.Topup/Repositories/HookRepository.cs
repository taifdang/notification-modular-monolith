
using API.Topup.Models;
using ShareCommon.Data;
using ShareCommon.Model;

namespace API.Topup.Repositories
{
    public class HookRepository : IHookRepository
    {
        private readonly DatabaseContext _db;
        public HookRepository(DatabaseContext db)
        {
            _db = db;
        }
        public async Task AddToInBox(string url,string type, string body)
        {
            await using var transaction = _db.Database.BeginTransaction();
            try
            {
                var inbox_topup_tbl = new InboxTopup
                {
                    event_type = type,
                    source = url,
                    payload = body,
                    create_at = DateTime.Now,
                };
                _db.inbox_topup.Add(inbox_topup_tbl);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex.ToString());   
            }
        }
    }
}
