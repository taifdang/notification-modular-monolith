
using API.Topup.Models;
using ShareCommon.Data;
using ShareCommon.Model;
using System.Text.Json;

namespace API.Topup.Repositories
{
    public class HookRepository : IHookRepository
    {
        private readonly DatabaseContext _db;
        private readonly ILogger<HookRepository> _logger;
        public HookRepository(DatabaseContext db, ILogger<HookRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task AddToInBox(string url,string type, string body)
        {
            await using var transaction = _db.Database.BeginTransaction();
            try
            {              
                var inbox_topup_tbl = new InboxTopup
                {
                    itopup_event_type = type,
                    itopup_source = url,
                    itopup_payload = body,
                    itopup_created_at = DateTime.Now,
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

        public async Task AddToInBox(string type, string body)
        {
            await using var transaction = _db.Database.BeginTransaction();
            try{
                //destructure
                var trans_id = Destructure(type,body);
                //check topup_id
                //if(await _db.inbox_topup.AnyAsync(x => x.topup_trans_id == topup_id))
                //{
                //    _logger.LogWarning($"[topup_api]:itopup_error >>{topup_id} is exist");
                //    return;
                //}
                //inbox_tbl
                var inbox_tbl = new InboxTopup
                {
                    itopup_trans_id = trans_id,
                    itopup_event_type = type,
                    itopup_source = "",
                    itopup_payload = body,
                    itopup_created_at = DateTime.Now,
                };
                _db.inbox_topup.Add(inbox_tbl);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                _logger.LogWarning($"[topup_api]:error >>{ex.ToString()}");
            }
        }

        public int Destructure(string type,string body)
        {
            switch (type) {
                case "sepay":
                    var transaction_id = JsonSerializer.Deserialize<SepayPayload>(body);
                    return transaction_id!.id;
                default:
                    return 0;
            }

        }
    }
}
