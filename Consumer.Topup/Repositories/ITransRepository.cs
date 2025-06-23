
namespace Consumer.Topup.Repositories
{
    public interface ITransRepository:IRepository<ShareCommon.Model.Topup>
    {
        Task AddTransaction(ShareCommon.Model.Topup transaction);
    }
}
