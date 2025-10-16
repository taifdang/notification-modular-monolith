
using BuildingBlocks.Exception;

namespace Notification.Application.Common.Exceptions;

public class RecipientNotFoundException : DomainException
{
    public RecipientNotFoundException() : base("Recipient not found")
    {
    }
}