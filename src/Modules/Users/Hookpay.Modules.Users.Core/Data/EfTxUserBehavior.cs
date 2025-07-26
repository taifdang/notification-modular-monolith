
using Hookpay.Shared.Core;
using Hookpay.Shared.EFCore;
using Hookpay.Shared.PersistMessageProcessor;
using Hookpay.Shared.Polly;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Transactions;

namespace Hookpay.Modules.Users.Core.Data;

public class EfTxUserBehavior<TRequest, TReponse> : IPipelineBehavior<TRequest, TReponse>
    where TRequest : notnull, IRequest<TReponse>
    where TReponse : notnull
{
    private readonly ILogger<EfTxUserBehavior<TRequest, TReponse>> _logger;
    private readonly UserDbContext _userDbContext;
    private readonly IPersistMessageDbContext _persistMessageDbContext;
    private readonly IEventDispatcher _eventDispatcher;

    public EfTxUserBehavior(
        ILogger<EfTxUserBehavior<TRequest, TReponse>> logger,
        UserDbContext userDbContext,
        IPersistMessageDbContext persistMessageDbContext,
        IEventDispatcher eventDispatcher)
    {
        _logger = logger;
        _userDbContext = userDbContext;
        _persistMessageDbContext = persistMessageDbContext;
        _eventDispatcher = eventDispatcher;
    }


    public async Task<TReponse> Handle(TRequest request, RequestHandlerDelegate<TReponse> next, CancellationToken cancellationToken)
    {

        _logger.LogInformation("{Prefix} Handled command with {TRequest}",
            GetType().Name,
            typeof(TRequest).FullName
            );

        _logger.LogDebug(
            "{Prefix} Handled command {TRequest} with content {TContent}",
            GetType().Name,
            typeof(TRequest).FullName,
            JsonSerializer.Serialize(request));

        var reponse = await next();

        _logger.LogInformation("{Prefix} excuted the {TRequest} request", 
            GetType().Name , 
            typeof(TRequest).FullName);    

        while (true)
        {
            var domainEvents = _userDbContext.GetDomainEvents();

            if(domainEvents is null || !domainEvents.Any())
                return reponse;

            _logger.LogInformation(
               "{Prefix} Open the transaction for {TRequest}",
               GetType().Name, 
               typeof(TRequest).FullName);

            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);

            await _eventDispatcher.SendAsync(domainEvents.ToArray(), typeof(IRequest), cancellationToken);

            //ref: https://www.codeproject.com/Articles/5378791/How-to-Use-Polly-In-Csharp-Easily-Handle-Faults-An
            await _userDbContext.RetryStrategyOnFailure(async () =>
            {
                await _userDbContext.SaveChangesAsync(cancellationToken);
            });

            await _persistMessageDbContext.RetryStrategyOnFailure(async () =>
            {
                await _persistMessageDbContext.SaveChangesAsync(cancellationToken);
            });

            scope.Complete();   

            return reponse;
        }
    }
}
