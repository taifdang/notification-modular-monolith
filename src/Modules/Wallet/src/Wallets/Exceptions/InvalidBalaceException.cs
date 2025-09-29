using BuildingBlocks.Exception;

namespace Wallet.Wallets.Exceptions;
public class InvalidBalaceException : DomainException
{
    public InvalidBalaceException() : base("Balance is not a negative value")
    {
    }
}
