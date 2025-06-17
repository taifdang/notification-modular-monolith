using ShareCommon.Model;
namespace Consumer.Topup.Repositories
{
    public interface IInboxRepository:IRepository<InboxTopup>
    {
        Task<List<InboxTopup>> GetListInbox();
        Task UpdateStatus(InboxTopup data);
    }
}
