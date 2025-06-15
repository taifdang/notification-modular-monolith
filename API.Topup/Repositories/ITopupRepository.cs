namespace API.Topup.Repositories
{
    public interface ITopupRepository:IDisposable
    {
        Task AddToInBox();
    }
}
