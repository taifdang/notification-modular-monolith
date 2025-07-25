using FluentValidation;
using Hookpay.Modules.Users.Core.Data;
using Hookpay.Modules.Users.Core.Users.Models;
using Hookpay.Shared.Core;
using Hookpay.Shared.Domain.Events;
using MediatR;

namespace Hookpay.Modules.Users.Core.Users.Features.RegisterNewUser;

public record RegisterNewUser(string Username, string Password, string ConfirmPassword , string Email, string Phone) : IRequest<RegisterNewUserResult>;

public record RegisterNewUserResult(int userId, string username, string email, decimal balance, string phone);

public record UserCreatedDomainEvent(int Id)  : IDomainEvent;

//ref: https://code-maze.com/fluentvalidation-in-aspnet/
public class RegisterNewUserValidator : AbstractValidator<RegisterNewUser>
{
    public RegisterNewUserValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Please enter field UserName");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Please enter field Password");
        RuleFor(x => x).Custom((x, context) =>
        {
            if(x.Password != x.ConfirmPassword)
            {
                context.AddFailure(nameof(x.Password),"Password not match");
            }
        });

        RuleFor(x => x.Phone).NotEmpty().WithMessage("Please enter field Phone")
                .Matches(@"^[0-9]+$").WithMessage("A valid number is required")
                .MaximumLength(13).WithMessage("Max number phone current is 13 digit");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter field Email")
                .EmailAddress().WithMessage("A valid message is required");
    }
}
public class RegisterNewUserHandler : IRequestHandler<RegisterNewUser, RegisterNewUserResult>
{
    private readonly UserDbContext _userDbContext;
    public readonly IEventDispatcher _eventDispatcher;

    public RegisterNewUserHandler(
        UserDbContext userDbContext,
        IEventDispatcher eventDispatcher)
    {
        _userDbContext = userDbContext;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<RegisterNewUserResult> Handle(RegisterNewUser request, CancellationToken cancellationToken)
    {       
        var user = Models.Users.Create(request.Username, request.Password, request.Email, request.Phone);      

        //get user id
        await _userDbContext.Users.AddAsync(user);
        
        await _userDbContext.SaveChangesAsync();

        //note: The internal command processor (via the event mapper) must be given a DomainEvent
        await _eventDispatcher.SendAsync(
            new UserCreatedDomainEvent(user.Id), 
            typeof(IInternalCommand), 
            cancellationToken);

        return new RegisterNewUserResult(user.Id, user.Username, user.Email, user.Balance, user.Phone);
    }
}
