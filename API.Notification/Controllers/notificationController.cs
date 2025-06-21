using API.Notification.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShareCommon.Generic;

namespace API.Notification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class notificationController : ControllerBase
    {
        private readonly MessageHub _hub;
        public notificationController(MessageHub hub)
        {
            _hub = hub;
        }
        [HttpPost]
        public async Task<IActionResult> Post(int user_id, string message)
        {
            try
            {
                await _hub.SendPersonalNotification(user_id,message);
                return Ok("Sent");
            }
            catch {
                return BadRequest("Fail");
            }
        }
        [HttpPost("push")]
        public async Task<IActionResult> Post([FromBody] TopupDetail detail)
        {
            Console.WriteLine(detail.user_id);  
            try
            {
                //await _hub.SendPersonalNotification(detail.user_id, detail.transfer_amount.ToString());
                await _hub.SendMessage(detail.transfer_amount.ToString());
                return Ok("Sent");
            }
            catch
            {
                return BadRequest("Fail");
            }
        }
    }
}
