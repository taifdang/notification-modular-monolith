using ShareCommon.Data;
using ShareCommon.Model;


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
