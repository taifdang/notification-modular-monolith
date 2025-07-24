using System.Net;

namespace Hookpay.Shared.Exceptions
{
    //ref: https://www.csharptutorial.net/csharp-tutorial/csharp-custom-exceptions/
    public class CustomException:System.Exception
    {
        public CustomException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, int? code = null):base(message) { }

        public HttpStatusCode StatusCode { get; }
        public int? Code { get; }

    }
}
