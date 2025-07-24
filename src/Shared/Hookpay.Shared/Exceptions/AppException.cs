
using System.Net;

namespace Hookpay.Shared.Exceptions;

public class AppException : CustomException
{
    public AppException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, int? code = null) : base(message, statusCode, code)
    {
    }
}
