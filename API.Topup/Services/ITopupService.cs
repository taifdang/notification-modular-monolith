namespace API.Topup.Services
{
    public interface ITopupService
    {
        //Convert payload format (type[thesieure/momo],payload[data])
        Task WebhookListener(string type,string body);
        string GetTypeWebHook(string url);
     
    }
}
