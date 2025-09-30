using BuildingBlocks.Exception;
using System.Net;

namespace Wallet.Wallets.Exceptions;

public class WalletNotFoundException : DomainException
{
    public WalletNotFoundException() : base("Wallet not found!", HttpStatusCode.NotFound)
    {
    }
}
