using Hookpay.Shared.Exceptions;

namespace Hookpay.Modules.Topups.Core.Topups.Exceptions
{
    public class InvalidTopupException : DomainException
    {
        public InvalidTopupException() : base("Data cannot be empty or null")
        {
        }
    }
}
