using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Shared.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message, int? code = null) : base(message, HttpStatusCode.BadRequest, code: code)
        {
        }
    }
}
