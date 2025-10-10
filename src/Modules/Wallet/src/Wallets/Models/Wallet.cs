using BuildingBlocks.Core.Model;
using Wallet.Wallets.ValueObjects;

namespace Wallet.Wallets.Models;

public record Wallet : Aggregate<WalletId>
{
    public Guid UserId { get; private set; } = default!;
    public string PaymentCode { get; private set; } = default!;
    public Balance Balance { get; private set; } = default!;
    
    public static Wallet Create(WalletId id, Guid userId ,string paymentCode, Balance balance, bool isDeleted = false)
    {
        var wallet = new Wallet
        {
            Id = id,
            UserId = userId,
            PaymentCode = paymentCode,
            Balance = balance,
            IsDeleted = isDeleted
        };

        return wallet;
    }

    public void Topup(Balance amount)
    {
        this.Balance = Balance.Of(Balance.Value + amount.Value);
    }
}
