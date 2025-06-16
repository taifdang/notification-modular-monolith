namespace API.Topup.Repositories
{
    public interface IHookRepository
    {
        Task AddToInBox(string url,string type,string body);
        Task AddToInBox(string type, string body);
        int Destructure(string type, string body);
    }
}
