namespace Wallet.Transactions.Features.GettingTransaction
{
    internal class GetTransaction
    {
    //    var transactions = await _dbContext.Transactions
    //.Join(
    //    _dbContext.Wallets,
    //    t => t.WalletId,
    //    w => w.Id,
    //    (t, w) => new { Transaction = t, Wallet = w }
    //)
    //.Where(x => x.Wallet.UserId == currentUserId)
    //.OrderByDescending(x => x.Transaction.CreatedAt)
    //.Select(x => new TransactionDto
    //{
    //    Id = x.Transaction.Id,
    //    Amount = x.Transaction.Amount,
    //    Type = x.Transaction.Type,
    //    CreatedAt = x.Transaction.CreatedAt,
    //    WalletId = x.Wallet.Id,
    //    WalletCode = x.Wallet.Code
    //})
    //.ToListAsync();
    }
}
