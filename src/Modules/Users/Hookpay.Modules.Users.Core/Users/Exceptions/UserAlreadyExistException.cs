
using Hookpay.Shared.Exceptions;
using System.Net;

namespace Hookpay.Modules.Users.Core.Users.Exceptions;

public class UserAlreadyExistException : AppException
{
    public UserAlreadyExistException(int? code = null) : base("User not exist!", code: code)
    {
    }
}
