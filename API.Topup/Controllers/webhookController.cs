using API.Topup.Models;
using API.Topup.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetResult()
        {
            //tao endpoint cho tung service goi toi 
            //xac thuc bang authorization
            //return ve type,format
            //check authorization
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return BadRequest(StatusCodes.Status401Unauthorized);
            }
            var apiKey = Request.Headers["Authorization"];
            if (apiKey != _config["ExternalService:Sepay:token"])
            {
                return BadRequest(StatusCodes.Status401Unauthorized);
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
                await topupService.WebhookListener("sepay",data);//type + data
                return Ok(data);          
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.ToString());
            }         
        }
    }
}
