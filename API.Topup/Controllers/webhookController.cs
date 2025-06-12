using API.Topup.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Topup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class webhookController : ControllerBase
    {
        private readonly ITopupService topupService;
        public webhookController(ITopupService topupService)
        {
            this.topupService = topupService;
        }
        [HttpPost]
        public async Task<IActionResult> GetResult()
        {
            try
            {
                var _request = Request;
                using var reader = new StreamReader(_request.Body);
                var body = await reader.ReadToEndAsync();
                var url = $"{_request.Scheme}://{_request.Host}{_request.PathBase}{_request.Path}{_request.QueryString}";
                await topupService.HandleWebhookListen(url, body);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
           
        }
    }
}
