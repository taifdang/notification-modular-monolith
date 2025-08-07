using System.Net;

namespace BuildingBlocks.Exception;

public class DomainException : AppException
{
    public DomainException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, int? code = null) : base(message, statusCode, code)
    {
    }
}
