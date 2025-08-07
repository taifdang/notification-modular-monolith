using BuildingBlocks.Exception;

namespace Topup.Topups.Exceptions;
public class InvalidTopupIdException : DomainException
{
    public InvalidTopupIdException(Guid topupId) 
        : base($"topupId: '{topupId}' is invalid")
    {
    }
}
