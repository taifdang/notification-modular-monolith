using BuildingBlocks.Constants;
using BuildingBlocks.Core;
using BuildingBlocks.Core.CQRS;
using FluentValidation;
using Identity.Identity.Exceptions;
using Identity.Identity.Models;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Identity.Features.RegisteringNewUser;

public record RegisterNewUser(string FirstName, string LastName,string UserName,string Email,string Password,string ConfirmPassword)
    : ICommand<RegisterNewUserResult>;

public record RegisterNewUserResult(Guid Id,string FirstName,string LastName,string UserName);

public record RegisterNewUserRequestDto(string FirstName,string LastName,string UserName,string Email,string Password,string ConfirmPassword);

public record RegisterNewUserResponseDto( Guid Id,string FirstName,string LastName,string UserName);

[ApiController]
[Route("api/identity")]
public class RegisterNewUserEndpoint(
    IMapper mapper,
    IMediator mediator) 
    : ControllerBase
{

    [HttpPost("register-user")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterNewUser(
        RegisterNewUser request, 
        CancellationToken cancellationToken)
    {
        //var command = mapper.Map<RegisterNewUser>(request);

        var result = await mediator.Send(request, cancellationToken);

        //var response = result.Adapt<RegisterNewUserResponseDto>();

        return Ok(result);
    }
}

public class RegisterNewUserValidator : AbstractValidator<RegisterNewUser>
{
    public RegisterNewUserValidator()
    {
        RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter the password");
        RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Please enter the confirm password");

        RuleFor(x => x).Custom((x, context) =>
        {
            if(x.Password != x.ConfirmPassword)
            {
                context.AddFailure(nameof(x.Password), "Password not match");
            }
        });

        RuleFor(x => x.UserName).NotEmpty().WithMessage("Please enter the username");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please enter the firstname");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Please enter the lastname");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter the email")
            .EmailAddress().WithMessage("A valid email is required");

    }
}

internal class RegisterNewUserHandler : ICommandHandler<RegisterNewUser, RegisterNewUserResult>
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly UserManager<User> _userManager;

    public RegisterNewUserHandler(
        IEventDispatcher eventDispatcher, 
        UserManager<User> userManager)
    {
        _eventDispatcher = eventDispatcher;
        _userManager = userManager;
    }

    public async Task<RegisterNewUserResult> Handle(RegisterNewUser request, CancellationToken cancellationToken)
    {
        if(request is null)
            throw new RegisterIdenttiyUserException("data empty");

        var applicationUser = new User()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
            PasswordHash = request.Password
        };

        var identityResult = await _userManager.CreateAsync(applicationUser, request.Password);
        var roleResult = await _userManager.AddToRoleAsync(applicationUser, IdentityConstant.Role.User);

        if (identityResult.Succeeded == false)
        {
            throw new RegisterIdenttiyUserException(string.Join(',',identityResult.Errors.Select(x=>x.Description)));
        }

        if (roleResult.Succeeded == false)
        {
            throw new RegisterIdenttiyUserException(string.Join(',', roleResult.Errors.Select(x => x.Description)));
        }

        //await _eventDispatcher.SendAsync(
        //    new UserCreated(
        //        applicationUser.Id,
        //        applicationUser.FirstName + " " + applicationUser.LastName),
        //    cancellationToken: cancellationToken);

        return new RegisterNewUserResult(
            applicationUser.Id,
            applicationUser.FirstName,
            applicationUser.LastName,
            applicationUser.UserName);

    }
}




