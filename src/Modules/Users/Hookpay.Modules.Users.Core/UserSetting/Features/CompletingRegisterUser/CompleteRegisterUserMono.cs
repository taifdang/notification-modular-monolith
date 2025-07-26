

using Hookpay.Modules.Users.Core.Data;
using Hookpay.Shared.Core.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hookpay.Modules.Users.Core.UserSetting.Features.CompletingRegisterUser;

public record CompleteRegisterUserMono(int Id) : IInternalCommand, IRequest;
public class CompleteRegisterUserMonoHandler : IRequestHandler<CompleteRegisterUserMono>
{
    private readonly UserDbContext _userDbContext;
   
    public CompleteRegisterUserMonoHandler(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }

    public async Task Handle(CompleteRegisterUserMono request, CancellationToken cancellationToken)
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
                await _userDbContext.UserSetting.AddAsync(new Users.Models.UserSetting { UserId = request.Id, AllowNotification = true });
            }

            await _userDbContext.SaveChangesAsync();
        }
        catch(Exception ex) 
        {
            throw new Exception("A valid in method UserSetting: {ex}", ex);
        }
      
    }
}
