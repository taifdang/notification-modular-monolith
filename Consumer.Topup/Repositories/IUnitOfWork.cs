
namespace Consumer.Topup.Repositories
{
    public interface IUnitOfWork:IDisposable
    { 
        ITransRepository trans {  get; }    
        IOutboxRepository outbox { get; }
        IUserRepository user { get; }
        IInboxRepository inbox { get; }
        Task CreateAsync();       
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangeAsync();
    }
}
