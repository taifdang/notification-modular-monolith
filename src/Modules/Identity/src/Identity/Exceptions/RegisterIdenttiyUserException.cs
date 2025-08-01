
using BuildingBlocks.Exception;
using System.Net;

namespace Identity.Identity.Exceptions;

public class RegisterIdenttiyUserException : AppException
{
    public RegisterIdenttiyUserException(string message) : base(message)
    {
    }
}
