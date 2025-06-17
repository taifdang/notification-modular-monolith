using ShareCommon.Model;


namespace Consumer.Topup.Repositories
{
    public interface IUserRepository:IRepository<Users>
    {
        Task<Users> FindUser(string username);
        Task UpdateBalance(int user_id, decimal balance);
    }
}
