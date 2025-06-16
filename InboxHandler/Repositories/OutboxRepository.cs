using ShareCommon.Data;
using ShareCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Topup.Repositories
{
    public class OutboxRepository : Repository<OutboxTopup>, IOutboxRepository
    {
        public OutboxRepository(DatabaseContext db) : base(db)
        {
        }

        public Task AddToOutBox(OutboxTopup outbox)
        {
            _db.Add(outbox);
            return Task.CompletedTask;
        }
    }
}
