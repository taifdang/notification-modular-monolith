using BuildingBlocks.Exception;
using System.Net;

namespace Topup.Topups.Exceptions;

public class TopupAlreadyExistException : AppException
{
    public TopupAlreadyExistException(int? code = default) 
        : base("Topup already exist!", HttpStatusCode.Conflict, code)
    {
    }
}
