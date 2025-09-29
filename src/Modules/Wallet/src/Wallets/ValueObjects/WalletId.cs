using Wallet.Wallets.Exceptions;

namespace Wallet.Wallets.ValueObjects;

public record WalletId
{
    public Guid Value { get; }

    private WalletId(Guid value)
    {
        Value = value;
    }
    public static WalletId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidWalletIdException(value);
        }

        return new WalletId(value);
    }

    public static implicit operator Guid(WalletId walletId)
    {
        return walletId.Value;
    }
}
