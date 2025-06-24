using API.Topup.Models;
using API.Topup.Services;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareCommon.Helper;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Topup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class webhookController : ControllerBase
    {
        private readonly ITopupService topupService;
        private readonly IConfiguration _config;
        public webhookController(ITopupService topupService, IConfiguration config)
        {
            this.topupService = topupService;
            this._config = config;
        }
        [HttpPost("sepay")]
        public async Task<StatusResponse<string>> GetResult()
        {
            //tao endpoint cho tung service goi toi 
            //xac thuc bang authorization
            //return ve type,format
            //check authorization
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return StatusResponse<string>.Failure("unauthorized");
            }
            var apiKey = Request.Headers["Authorization"];
            if (apiKey != _config["ExternalService:Sepay:token"])
            {
                return StatusResponse<string>.Failure("unauthorized");
            }           
            try
            {
                #region command #ea321
                //var _request = Request;
                //using var reader = new StreamReader(_request.Body);
                //var body = await reader.ReadToEndAsync();
                //var url = $"{_request.Scheme}://{_request.Host}{_request.PathBase}{_request.Path}{_request.QueryString}";
                //await topupService.WebhookListener(url, body);
                #endregion
                //read body when know format json request 
                var requestBody = await HttpContext.Request.ReadFromJsonAsync<SepayPayload>();
                var data = JsonSerializer.Serialize(requestBody);
                var response = await topupService.WebhookListener("sepay",data);//type + data
                if(response.success is true) return StatusResponse<string>.Success(response.data);
                return StatusResponse<string>.Failure(response.Message);
            }
            catch(Exception ex) 
            {
                return StatusResponse<string>.Failure(ex.ToString());
            }         
        }
    }
}
