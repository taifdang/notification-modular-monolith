using Hookpay.Modules.Users.Core.Users.Dao;
using Hookpay.Modules.Users.Core.Users.Dtos;
using Hookpay.Modules.Users.Core.Users.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Users.Core.Users.Features.SignUp;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _repository;
    public CreateUserCommandHandler(IUserRepository repository) {  _repository = repository; }
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new Models.Users 
        { 
            user_name = request.username,
            user_password = request.password,
            user_email = request.email,
            user_phone = request.phone,
        };
        await _repository.AddAsync(user); 
        return new UserDto { username = user.user_name,email = user.user_email, phone = user.user_phone};
    }
}
