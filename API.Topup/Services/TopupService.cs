using API.Topup.Repositories;
using ShareCommon.Helper;

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
        public async Task<StatusResponse<string>> WebhookListener(string type,string body)
        {           
            //var type = GetTypeWebHook(url);//strategy,factory pattern .... >> mapping options
            var data = await _hookRepository.AddToInBox(type,body);//*method chung
            if (data.success is true) return StatusResponse<string>.Success(data.data);
            return StatusResponse<string>.Failure(data.Message);
        }
    }
}
