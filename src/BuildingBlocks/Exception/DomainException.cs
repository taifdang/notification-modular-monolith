using System.Net;

namespace BuildingBlocks.Exception;

public class DomainException : CustomException
{
    public DomainException(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest) 
        : base(message, statusCode)
    {
    }
}
