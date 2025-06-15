
namespace API.Topup.Repositories
{
    public class TopupRepository : ITopupRepository
    {
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _provider;
        public TopupRepository(IServiceScopeFactory provider)
        {
            _provider = provider;   
        }
        public Task AddToInBox()
        {
           
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
