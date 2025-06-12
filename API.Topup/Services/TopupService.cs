using API.Topup.Repositories;
using Azure.Core;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Text.Json;

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
        public string GetWebHookType(string url)
        {
            switch (url)
            {              
                //case:...
                default:
                    return "sepay";
            }         
        }
        public async Task HandleWebhookListen(string url,string body)
        {
           
            var type = GetWebHookType(url);
            await _hookRepository.AddToInBox(url,type,body);      
            Console.WriteLine(body);
        }
    }
}
