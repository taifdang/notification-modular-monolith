using ShareCommon.Helper;

namespace API.Topup.Services
{
    public interface ITopupService
    {
        //Convert otopup_payload format (type[thesieure/momo],otopup_payload[data])
        Task<StatusResponse<string>> WebhookListener(string type,string body);
        string GetTypeWebHook(string url);
     
    }
}
