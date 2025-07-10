using Hookpay.Modules.Users.Core.Users.Features.SignUp;
using Hookpay.Shared.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hookpay.Modules.Users.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _context;
    public UsersController(IMediator mediator,IHttpContextAccessor context) {  _mediator = mediator;_context = context; }
    [HttpGet("sign-in")]
    //[Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult SignIn()
    {
        //return access token
        //var result = _mediator.Send();
        return Ok();
    }
    [HttpPost("sign-up")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Result<object>> SignUp(CreateUserCommand command,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command,cancellationToken);
        if (result is null) return Result<object>.Failure();
        return Result<object>.Success(result);
    }

}
