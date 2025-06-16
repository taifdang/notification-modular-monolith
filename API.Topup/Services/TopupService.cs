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

        public string GetTypeWebHook(string url)
        {
            throw new NotImplementedException();
        }

        //public string GetTypeWebHook(string url)
        //{
        //    //[#note]:
        //    //- mapping du lieu khi nhan,
        //    //- phan biet bang <domain> 
        //    //- xac thuc token
        //    //- return format object
        //    switch (url)
        //    {              
        //        //case:...
        //        default:
        //            return "sepay";
        //    }         
        //}
        public async Task WebhookListener(string type,string body)
        {           
            //var type = GetTypeWebHook(url);//strategy,factory pattern .... >> mapping options
            await _hookRepository.AddToInBox(type,body);//*method chung      
        }
    }
}
