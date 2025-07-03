using API.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customer;
        public CustomerController(ICustomerService customer)
        {
            _customer = customer;
        }

        [HttpPost("signin")]
        public IActionResult SignIn(string username)
        {
            try
            {
                var value = _customer.GenerateJwtToken(username);
                return Ok(value);
            }
            catch
            {
                return BadRequest("Error");
            }
        }
        [HttpPost("signup")]
        public IActionResult SignUp()
        {
            return Ok();
        }
        [HttpPost("forgetpassword")]
        public IActionResult ForgetPassword()
        {
            return Ok();
        }
        [Authorize,HttpPost("test")]
        public IActionResult Test()
        {
            return Ok();
        }

    }
}
