
using Hookpay.Shared.Exceptions;

namespace Hookpay.Modules.Users.Core.Users.Exceptions
{
    public class InvalidPaginationParameter : AppException
    {
        public InvalidPaginationParameter(int? code = default) : base("PageNumber or PageSize is invalid", code: code)
        {
        }
    }
}
