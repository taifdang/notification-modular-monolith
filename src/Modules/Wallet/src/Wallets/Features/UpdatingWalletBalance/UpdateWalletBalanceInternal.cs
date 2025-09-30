
using Ardalis.GuardClauses;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallet.Data;
using Wallet.Wallets.Exceptions;
using Wallet.Wallets.ValueObjects;

namespace Wallet.Wallets.Features.UpdatingWalletBalance;

public record UpdateWalletBalanceInternalCommand(Guid Id, Guid WalletId, decimal Amount) : InternalCommand;
internal class UpdateWalletBalanceInternalHandler : ICommandHandler<UpdateWalletBalanceInternalCommand>
{
    private readonly WalletDbContext _walletDbContext;

    public UpdateWalletBalanceInternalHandler(WalletDbContext walletDbContext)
    {
        _walletDbContext = walletDbContext;
    }

    public async Task<Unit> Handle(UpdateWalletBalanceInternalCommand request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        using var transaction = await _walletDbContext.Database.BeginTransactionAsync(cancellationToken);

        try 
        { 
            var wallet = await _walletDbContext.Wallets.SingleOrDefaultAsync(x => x.Id == request.WalletId, cancellationToken) 
                ?? throw new WalletNotFoundException(); 
            
            wallet.Topup(Balance.Of(request.Amount)); 
            
            var transactionEntity = await _walletDbContext.Transactions.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken) 
                ?? throw new TransactionNotExistException(request.Id);
            
            transactionEntity.ChangeState(Transactions.Enums.TransactionStatus.Successed); 
            
            await _walletDbContext.SaveChangesAsync(cancellationToken); 
            await transaction.CommitAsync(cancellationToken); 
            return Unit.Value; 
        } 
        catch 
        { 
            await transaction.RollbackAsync(cancellationToken); 
            throw; 
        }
    }
}

