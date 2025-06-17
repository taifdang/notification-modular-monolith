using API.Topup.Repositories;

namespace API.Topup.Services
{
    public class TopupService : ITopupService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IHookRepository _hookRepository;
        public TopupService(
        IHttpContextAccessor context,
        IHookRepository hookRepository
        )
        {
            this._context = context;
            this._hookRepository = hookRepository;
        }

        public string GetTypeWebHook(string url)
        {
            throw new NotImplementedException();
        }    
        public async Task WebhookListener(string type,string body)
        {           
            //var type = GetTypeWebHook(url);//strategy,factory pattern .... >> mapping options
            await _hookRepository.AddToInBox(type,body);//*method chung      
        }
    }
}
