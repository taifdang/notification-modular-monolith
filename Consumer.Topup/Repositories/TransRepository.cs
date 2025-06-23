using ShareCommon.Data;

namespace Consumer.Topup.Repositories
{
    public class TransRepository : Repository<ShareCommon.Model.Topup>,ITransRepository
    {
        public TransRepository(DatabaseContext db) : base(db)
        {
        }

        public async Task AddTransaction(ShareCommon.Model.Topup transaction)
        {
           _db.Add(transaction);    
           await _db.SaveChangesAsync();
        }
    }
}
