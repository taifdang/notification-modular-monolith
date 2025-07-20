using Hookpay.Shared.Exceptions;

namespace Hookpay.Modules.Topups.Core.Exceptions
{
    public class InvalidNameException : BadRequestException
    {
        public InvalidNameException() : base("Username cannot be convert")
        {
        }
    }
}
