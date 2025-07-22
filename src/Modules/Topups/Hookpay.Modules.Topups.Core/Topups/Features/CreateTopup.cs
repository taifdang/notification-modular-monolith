using Hookpay.Modules.Topups.Core.Data;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Domain.Events;
using Hookpay.Shared.EventBus;
using Hookpay.Modules.Topups.Core.Topups.Exceptions;
using MediatR;
using Hookpay.Shared.Core;

namespace Hookpay.Modules.Topups.Core.Topups.Features
{
    public record CreateTopup(
    string gateway,
    string transactionDate,
    string? accountNumber,
    string? subAccount,
    string? code,
    string? content,
    string transferType,
    string description,
    int transferAmount,
    string? referenceCode,
    decimal accumulated,
    int id) : IRequest<CreateTopupResult>;
    
    public record CreateTopupResult(int id, string accountNumber, string transferType, decimal transferAmount);

    //
    public record TopupCreatedDomainEvent(int transId, string creator, decimal tranferAmount) : IHaveIntegrationEvent;

    public class CreateTopupHandler : IRequestHandler<CreateTopup, CreateTopupResult>
    {
        private readonly TopupDbContext _topupDbContext;
        private readonly IBusPublisher _publisher;
        private readonly IEventDispatcher _eventDispatcher;

        public CreateTopupHandler(
            TopupDbContext topupDbContext, 
            IBusPublisher publisher,
            IEventDispatcher eventDispatcher)
        {
            _topupDbContext = topupDbContext;
            _publisher = publisher;
            _eventDispatcher = eventDispatcher;
        }
        public async Task<CreateTopupResult> Handle(CreateTopup request, CancellationToken cancellationToken)
        {
            if(request is null)
            {
                throw new InvalidNameException();
                
            }

            var creator = request.description.Split("NAPTIEN ")[1].ToLower();

            if (string.IsNullOrWhiteSpace(creator))
            {
                throw new InvalidNameException();
            }

            var topupEntity = Models.Topup.Create(request.id, creator, request.transferAmount);

            var newTopup = (await _topupDbContext.Topup.AddAsync(topupEntity, cancellationToken)).Entity;

            //note: processor method publish and savechange via domainevent
            await _topupDbContext.SaveChangesAsync(cancellationToken);

            await _eventDispatcher.SendAsync(
                new TopupCreated(topupEntity.TransactionId, topupEntity.Creator, topupEntity.TransferAmount),
                cancellationToken: cancellationToken            
                );
            //await _publisher.SendAsync(new TopupCreated(topupEntity.TransactionId, topupEntity.Creator, topupEntity.TransferAmount), cancellationToken);         

            return new CreateTopupResult(newTopup.TransactionId, request.transferType, request.transferType, request.transferAmount);
        }
    }
}
