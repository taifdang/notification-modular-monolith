
using Hookpay.Shared.Exceptions;


namespace Hookpay.Modules.Users.Core.Topups.Exceptons
{
    public class MessageNotFoundException : AppException
    {
        public MessageNotFoundException(int? code = default) : base("Message not found", code: code)
        {
        }
    }
}
