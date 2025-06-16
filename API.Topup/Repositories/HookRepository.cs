
using API.Topup.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ShareCommon.Data;
using ShareCommon.Model;
using System;
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

        public async Task AddToInBox(string type, string body)
        {
            await using var transaction = _db.Database.BeginTransaction();
            try{
                //destructure
                var trans_id = Destructure(type,body);
                //check trans_id
                //if(await _db.inbox_topup.AnyAsync(x => x.transaction_id == trans_id))
                //{
                //    _logger.LogWarning($"[topup_api]:error >>{trans_id} is exist");
                //    return;
                //}
                //inbox_tbl
                var inbox_tbl = new InboxTopup
                {
                    transaction_id = trans_id,
                    event_type = type,
                    source = "",
                    payload = body,
                    create_at = DateTime.Now,
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
