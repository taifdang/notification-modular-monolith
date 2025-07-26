
using Hookpay.Shared.Core;
using Hookpay.Shared.PersistMessageProcessor;
using Hookpay.Shared.Polly;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace Hookpay.Shared.EFCore;

public class EfTxBehevior<TRequest, TResponse>(
    ILogger<EfTxBehevior<TRequest, TResponse>> logger,
    IDbContext dbContext,
    IPersistMessageDbContext persistMessageDbContext,
    IEventDispatcher eventDispatcher
    ) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {


        //ref: https://learn.microsoft.com/en-us/ef/core/saving/transactions#using-systemtransactions
        using var scope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);

        var reponse = await next();

        while (true)
        {
            var domainEvents = dbContext.GetDomainEvents();

            if (domainEvents is null || !domainEvents.Any())
                return reponse;

            await eventDispatcher.SendAsync(domainEvents.ToArray(), typeof(IRequest), cancellationToken);

            //ref:
            await dbContext.RetryStrategyOnFailure(async () =>
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            });

            await persistMessageDbContext.RetryStrategyOnFailure(async () =>
            {
                await persistMessageDbContext.SaveChangesAsync();
            });
            
            scope.Complete();

            return reponse;
        }

        
       
    }
}
