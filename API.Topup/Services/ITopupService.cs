namespace API.Topup.Services
{
    public interface ITopupService
    {
        //Convert payload format (type[thesieure/momo],payload[data])
        Task HandleWebhookListen(string url,string body);
        string GetWebHookType(string url);
     
    }
}
