using BuildingBlocks.Exception;

namespace Topup.Topups.Exceptions;
public class InvalidNameException : DomainException
{
    public InvalidNameException() 
        : base("CreateByName is not empty")
    {
    }
}
