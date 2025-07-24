using Hookpay.Modules.Topups.Core.Data;
using Hookpay.Shared.Contracts;
using Hookpay.Shared.Domain.Events;
using Hookpay.Shared.EventBus;
using Hookpay.Modules.Topups.Core.Topups.Exceptions;
using MediatR;
using Hookpay.Shared.Core;
using FluentValidation;

namespace Hookpay.Modules.Topups.Core.Topups.Features
{
    public record CreateTopup(
    string Gateway,
    string TransactionDate,
    string AccountNumber,
    string? SubAccount,
    string? Code,
    string? Content,
    string TransferType,
    string Description,
    int TransferAmount,
    string? ReferenceCode,
    decimal Accumulated,
    int Id) : IRequest<CreateTopupResult>;
    
    public record CreateTopupResult(int Id, string AccountNumber, string TransferType, decimal TransferAmount);

    public record TopupCreatedDomainEvent(int TransId, string Creator, decimal TransferAmount) : IHaveIntegrationEvent;

    public class CreateTopupValidator : AbstractValidator<CreateTopup> 
    {
        public CreateTopupValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Please enter field Id");

            RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Please enter field AccountNumber")
                    .Matches(@"^[0-9]+$").WithMessage("A valid AccountNumber required");

            RuleFor(x => x.TransferAmount).NotEmpty().WithMessage("Please enter field TransferAmount");
        }
    }


    public class CreateTopupHandler : IRequestHandler<CreateTopup, CreateTopupResult>
    {
        private readonly TopupDbContext _topupDbContext;
        private readonly IEventDispatcher _eventDispatcher;

        public CreateTopupHandler(
            TopupDbContext topupDbContext, 
            IBusPublisher publisher,
            IEventDispatcher eventDispatcher)
        {
            _topupDbContext = topupDbContext;
            _eventDispatcher = eventDispatcher;
        }
        public async Task<CreateTopupResult> Handle(CreateTopup request, CancellationToken cancellationToken)
        {          

            var creator = request.Description.Split("NAPTIEN ")[1].ToLower();

            if (string.IsNullOrWhiteSpace(creator))
            {
                throw new InvalidNameException();
            }

            var topupEntity = Models.Topup.Create(request.Id, creator, request.TransferAmount);

            var newTopup = (await _topupDbContext.Topup.AddAsync(topupEntity, cancellationToken)).Entity;

            //note: processor method publish and savechange via domainevent
            await _topupDbContext.SaveChangesAsync(cancellationToken);

            await _eventDispatcher.SendAsync(
                new TopupCreated(topupEntity.TransactionId, topupEntity.Creator, topupEntity.TransferAmount),
                cancellationToken: cancellationToken            
                );
            //await _publisher.SendAsync(new TopupCreated(topupEntity.TransactionId, topupEntity.Creator, topupEntity.TransferAmount), cancellationToken);         

            return new CreateTopupResult(newTopup.TransactionId, request.TransferType, request.TransferType, request.TransferAmount);
        }
    }
}
