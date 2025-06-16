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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _db;     
        public ITransRepository trans { get; }
        public IOutboxRepository outbox { get; }
        public IUserRepository user { get; }
        public IInboxRepository inbox { get; }

        public UnitOfWork(DatabaseContext db)
        {
            _db = db;
            trans = new TransRepository(db);
            outbox = new OutboxRepository(db);
            user = new UserRepository(db);  
            inbox = new InboxRepository(db);    
        }
        public async Task CommitAsync()
        {
            await _db.Database.CommitTransactionAsync();
        }
        public async Task CreateAsync()
        {
            await _db.Database.BeginTransactionAsync();
        }
        public void Dispose()
        {
            _db?.Dispose(); 
        }
        public async Task RollbackAsync()
        {
            await _db.Database.RollbackTransactionAsync();
        }
        public async Task SaveChangeAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
