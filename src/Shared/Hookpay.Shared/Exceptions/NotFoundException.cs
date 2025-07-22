using System.Net;

namespace Hookpay.Shared.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message, int? code = null) : base(message, HttpStatusCode.InternalServerError, code: code)
        {
        }
    }
}
