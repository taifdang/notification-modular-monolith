
using Ardalis.GuardClauses;
using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Utils;
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
    private readonly IEventDispatcher _eventDispatcher;

    public UpdateWalletBalanceInternalHandler(WalletDbContext walletDbContext, IEventDispatcher eventDispatcher)
    {
        _walletDbContext = walletDbContext;
        _eventDispatcher = eventDispatcher;
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

            if(transactionEntity.TransactionType == Transactions.Enums.TransactionType.Topup)
            {
                await _eventDispatcher.SendAsync(new PersonalNotificationRequested(NotificationType.Topup,new Recipient(wallet.UserId,""),
                    DictionaryExtensions.SetPayloads(("topupId", transactionEntity.ReferenceCode),("userId", wallet.UserId)), 
                    NotificationPriority.High));
            }

            return Unit.Value; 
        } 
        catch 
        { 
            await transaction.RollbackAsync(cancellationToken); 
            throw; 
        }
    }
}

