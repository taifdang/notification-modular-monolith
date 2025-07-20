using Hookpay.Modules.Users.Core.Users.Dtos;
using Hookpay.Modules.Users.Core.Users.Features.GetAvailableUsers;
using Hookpay.Modules.Users.Core.Users.Features.RegisterNewUser;
using Hookpay.Modules.Users.Core.Users.Features.SignIn;
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
    public async Task<Result<TokenReponse>> SignIn(SignInCommand command,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command,cancellationToken);
        if (string.IsNullOrEmpty(result)) return Result<TokenReponse>.Failure();        
        return Result<TokenReponse>.Success(new TokenReponse { accessToken = result});
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
    [HttpPost("WeatherForCast")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetWeather()
    {
        var data = new GetAvailableUsers();
        var result = await _mediator.Send(data);
        
        return Ok(result);
        //return $"hello world {Random.Shared.Next(1,20).ToString()}";
    }

}
