
using FluentValidation;
using Hookpay.Modules.Users.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hookpay.Modules.Users.Core.UserSetting.Features.UpdateUserSetting;

public record UpdateUserSetting(int UserId, bool AllowNotification) : IRequest<UpdateUserSettingResult>;

public record UpdateUserSettingResult(string status);

public class UpdateUserSettingValidator : AbstractValidator<UpdateUserSetting>
{
    public UpdateUserSettingValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Please enter field UserId");

        RuleFor(x => x.AllowNotification).NotEmpty().WithMessage("Please enter field AllowNotification");
    }
}

public class UpdateUserSettingHandler : IRequestHandler<UpdateUserSetting, UpdateUserSettingResult>
{
    private readonly UserDbContext _userDbContext;

    public UpdateUserSettingHandler(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext; 
    }

    public async Task<UpdateUserSettingResult> Handle(UpdateUserSetting request, CancellationToken cancellationToken)
    {
        var user = await _userDbContext.UserSetting
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        if(user.Users?.Status == Users.Enums.UserStatus.Locked)
        {
            return new UpdateUserSettingResult("user is locked");
        }

        if (user is not null)
        {
            user.ChangeSettings(request.AllowNotification);
        }
        else
        {
            _userDbContext.UserSetting.Add(new Users.Models.UserSetting { UserId = user.Id, AllowNotification = true });
        }

        await _userDbContext.SaveChangesAsync();

        return new UpdateUserSettingResult("Changed setting successfully");
        
    }
}
