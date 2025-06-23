using ShareCommon.Model;

namespace Consumer.Topup.Repositories
{
    public interface IOutboxRepository: IRepository<OutboxTopup>
    {
        Task AddToOutBox(OutboxTopup outbox);
    }
}
