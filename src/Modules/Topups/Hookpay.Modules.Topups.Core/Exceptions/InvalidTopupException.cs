using Hookpay.Shared.Exceptions;

namespace Hookpay.Modules.Topups.Core.Exceptions
{
    public class InvalidTopupException : BadRequestException
    {
        public InvalidTopupException() : base("Data cannot be null or empty")
        {
        }
    }
}
