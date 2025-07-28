

using System.Net;

namespace Hookpay.Shared.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException(string message, int? code = null) : base(message, HttpStatusCode.BadRequest, code: code)
        {
        }
    }
}
