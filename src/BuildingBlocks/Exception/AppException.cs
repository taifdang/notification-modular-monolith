
using System.Net;

namespace BuildingBlocks.Exception;

public class AppException : CustomException
{
    public AppException(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest, 
        int? code = null) : base(message, statusCode, code)
    {
    }
}
