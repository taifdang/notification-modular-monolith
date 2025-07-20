using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Exceptions
{
    public class CustomException:System.Exception
    {
        public CustomException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, int? code = null):base(message) { }

        public HttpStatusCode StatusCode { get; }
        public int? Code { get; }

    }
}
