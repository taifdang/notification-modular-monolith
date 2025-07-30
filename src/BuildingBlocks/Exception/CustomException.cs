
using System.Net;
namespace BuildingBlocks.Exception;

public class CustomException : System.Exception
{
    public CustomException(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
        int? code = null) : base(message) 
    {
        StatusCode = statusCode;
        Code = code;
    }

    public HttpStatusCode StatusCode { get; }
    public int? Code {  get; }
}
