using BuildingBlocks.Core.Model;
using Wallet.Transactions.Enums;
using Wallet.Transactions.ValueObjects;
using Wallet.Wallets.ValueObjects;

namespace Wallet.Transactions.Models;

public record Transaction : Aggregate<TransactionId>
{
    public WalletId WalletId { get; private set; }
    public decimal Amount { get; private set; } = default!;
    public string? AccountNumber { get; private set; }
    public int? ReferenceCode { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public TransactionStatus TransactionStatus { get; private set; }

    public static Transaction Create(TransactionId id, WalletId walletId, decimal amount, string accountNumber,
        int referenceCode, TransactionType transactionType, TransactionStatus transactionStatus, bool isDeleted = false)
    {
        var transaction = new Transaction
        {
            Id = id,
            WalletId = walletId,
            Amount = amount,
            AccountNumber = accountNumber,
            ReferenceCode = referenceCode,
            TransactionType = transactionType,
            TransactionStatus = transactionStatus,
            IsDeleted = isDeleted
        };

        return transaction;
    }

    public void ChangeState(TransactionStatus status)
    {
        TransactionStatus = status;
    }
}
