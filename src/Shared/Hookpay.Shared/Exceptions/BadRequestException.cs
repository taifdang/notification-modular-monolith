using System.Net;

namespace Hookpay.Shared.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message, int? code = null) : base(message, HttpStatusCode.BadRequest, code: code)
        {
        }
    }
}
