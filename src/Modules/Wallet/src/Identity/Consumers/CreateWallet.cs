
using BuildingBlocks.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Wallet.Data;
using Wallet.Extensions.Infrastructure;
using Wallet.Wallets.Exceptions;
using Wallet.Wallets.ValueObjects;

namespace Wallet.Identity.Consumers;

public class CreateWallet : IConsumer<UserCreated>
{
    private readonly WalletDbContext _walletDbContext;

    public CreateWallet(WalletDbContext walletDbContext)
    {
        _walletDbContext = walletDbContext;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var wallet = await _walletDbContext.Wallets.SingleOrDefaultAsync(x => x.UserId == context.Message.Id);
        if (wallet is not null)
        {
            throw new WalletAlreadyExistException();
        }

        //with a small number of payment codes, retry if duplicate (36^8 = 2.8211099e+12 digits) + index / unique on db
        //retry 3 times then throw exception
        var paymentCode = WalletExtensions.GeneratePaymentCode();

        var walletEnitty = Wallets.Models.Wallet.Create(WalletId.Of(NewId.NextGuid()), paymentCode, Balance.Of(0));

        await _walletDbContext.Wallets.AddAsync(walletEnitty);

        await _walletDbContext.SaveChangesAsync();
    }
}
