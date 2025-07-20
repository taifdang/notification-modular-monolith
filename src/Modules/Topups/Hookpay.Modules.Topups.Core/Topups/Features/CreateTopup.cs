using Hookpay.Modules.Topups.Core.Data;
using Hookpay.Modules.Topups.Core.Exceptions;
using Hookpay.Modules.Topups.Core.Topups.Dtos;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Domain.Events;
using Hookpay.Shared.EventBus;
using MediatR;

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

    public record CreateTopupDomainEvent(string creator, decimal tranferAmount) : IDomainEvent;

    public class CreateTopupHandler : IRequestHandler<CreateTopup, CreateTopupResult>
    {
        private readonly TopupDbContext _topupDbContext;
        private readonly IBusPublisher _publisher;

        public CreateTopupHandler(TopupDbContext topupDbContext, IBusPublisher publisher)
        {
            _topupDbContext = topupDbContext;
            _publisher = publisher;
        }
        public async Task<CreateTopupResult> Handle(CreateTopup request, CancellationToken cancellationToken)
        {
            if(request is null)
            {
                throw new InvalidTopupException();
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

            await _publisher.SendAsync<TopupContracts>(new TopupContracts(topupEntity.TransactionId, topupEntity.Creator, topupEntity.TransferAmount), cancellationToken);         

            return new CreateTopupResult(newTopup.TransactionId, request.transferType, request.transferType, request.transferAmount);
        }
    }
}
