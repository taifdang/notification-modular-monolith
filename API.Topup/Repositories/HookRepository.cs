
using API.Topup.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;
using ShareCommon.Helper;
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

        public async Task<StatusResponse<string>> AddToInBox(string type, string body)
        {
            await using var transaction = _db.Database.BeginTransaction();
            try{
                //destructure
                var trans_id = Destructure(type,body);            
                var inbox_tbl = new InboxTopup
                {
                    itopup_id = trans_id,
                    itopup_event_type = type,
                    itopup_source = "",
                    itopup_payload = body,
                    itopup_created_at = DateTime.Now,
                };
                _db.inbox_topup.Add(inbox_tbl);
                await _db.SaveChangesAsync();
                transaction.Commit();
                return StatusResponse<string>.Success(body);
            }
            catch(DbUpdateException db_err)
            {
                return StatusResponse<string>.Failure("violet db");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogWarning($"[topup_api]:error >>{ex.ToString()}");
                return StatusResponse<string>.Failure(ex.ToString());
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
