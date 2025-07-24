

using Hookpay.Modules.Users.Core.Data;
using Hookpay.Shared.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hookpay.Modules.Users.Core.UserSetting.Features.CompletingRegisterUser;

public record CompleteUserRegisterUserMono(int Id) : IInternalCommand, IRequest;
public class CompleteUserRegisterUserMonoHandler : IRequestHandler<CompleteUserRegisterUserMono>
{
    private readonly UserDbContext _userDbContext;
   
    public CompleteUserRegisterUserMonoHandler(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }

    public async Task Handle(CompleteUserRegisterUserMono request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userDbContext.UserSetting
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.UserId == request.Id, cancellationToken);

            //update
            if (user is not null)
            {
                await _userDbContext.UserSetting
                    .Where(x => x.UserId == request.Id)
                    .ExecuteUpdateAsync(x => x.SetProperty(x => x.AllowNotification, true));
            }
            else
            {
                _userDbContext.UserSetting.Add(new Users.Models.UserSetting { UserId = user.Id, AllowNotification = true });
            }

            await _userDbContext.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            throw new Exception("A valid in method UserSetting: {ex}", ex);
        }
      
    }
}
