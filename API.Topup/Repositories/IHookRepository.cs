namespace API.Topup.Repositories
{
    public interface IHookRepository
    {
        Task AddToInBox(string url,string type,string body);
    }
}
