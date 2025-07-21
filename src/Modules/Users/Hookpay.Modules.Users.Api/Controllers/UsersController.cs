
using Hookpay.Modules.Users.Core.Users.Features.GetAvailableUsers;
using Hookpay.Modules.Users.Core.Users.Features.RegisterNewUser;
using Hookpay.Modules.Users.Core.Users.Features.UserSignedIn;
using Hookpay.Shared.Utils;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hookpay.Modules.Users.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _context;
  
    public UsersController(IMediator mediator,IHttpContextAccessor context) 
    {  
        _mediator = mediator;
        _context = context;    
    }
    [HttpPost("sign-in")]
    //[Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Result<UserSignedInResult>> SignIn(UserSignedIn command,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command,cancellationToken);

        if (string.IsNullOrEmpty(result.accessToken))
        {
            return Result<UserSignedInResult>.Failure();
        }       

        return Result<UserSignedInResult>.Success(result);
    }
    [HttpPost("sign-up")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Result<RegisterNewUserResult>> RegisterUser(RegisterNewUser command,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command,cancellationToken);
        if (result is null) return Result<RegisterNewUserResult>.Failure();
        return Result<RegisterNewUserResult>.Success(result);
    }
    [HttpPost("get-available-users")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetWeather()
    {
        var result = await _mediator.Send(new GetAvailableUsers(1,10));
        var response = result.Adapt<GetAvailableUsersReponse>();
        return Ok(response);      
    }
    [HttpGet("/")]
    public string GetGRPC() => "connected grpc ......";

}
