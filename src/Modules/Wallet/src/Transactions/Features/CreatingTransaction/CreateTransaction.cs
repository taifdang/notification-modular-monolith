using Ardalis.GuardClauses;
using BuildingBlocks.Core;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Utils;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Data;
using Wallet.Extensions.Infrastructure;
using Wallet.Transactions.Exceptions;
using Wallet.Transactions.ValueObjects;
using Wallet.Wallets.ValueObjects;

namespace Wallet.Transactions.Features.CreatingTransaction;

public record CreateTransaction(int ReferenceCode, string AccountNumber, string Description, decimal Amount) : ICommand<CreateTransactionResult>
{
    public Guid Id { get; init; } = NewId.NextGuid();
}

public record CreateTransactionResult(Guid Id, int ReferenceCode);

public record TransactionCreatedDomainEvent(Guid Id, Guid WalletId, int ReferenceCode, decimal Amount) : IDomainEvent;

public record CreateTransactionRequestDto(string Gateway, string TransactionDate, string AccountNumber,
    string? SubAccount, string? Code, string Content, string TransferType, string Description,
    decimal TransferAmount, int Accumulated, int Id);
public record CreateTransactionResponseDto(Guid Id, int ReferenceCode);

[ApiController]
[Route("api/transaction")]
public class CreateTransactionEndpoint : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper mapper;
    public CreateTransactionEndpoint(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        this.mapper = mapper;
    }
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Result<CreateTransactionResponseDto>> CreateTransactionRequest(
        CreateTransactionRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(mapper.Map<CreateTransaction>(request), cancellationToken);
        return result is null
            ? Result<CreateTransactionResponseDto>.Failure("Transaction creation failed")
            : Result<CreateTransactionResponseDto>.Success(result.Adapt<CreateTransactionResponseDto>());
    }
}

public class CreateTransactionValidator : AbstractValidator<CreateTransaction>
{
    public CreateTransactionValidator()
    {

        RuleFor(x => x.Id).NotEmpty().WithMessage("ReferenceCode must be greater than zero");
        RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Please enter field AccountNumber")
                    .Matches(@"^[0-9]+$").WithMessage("A valid AccountNumber required");      
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Transfer amount must be greater than zero");
    }
}

internal class CreateTransactionCommandHandler : ICommandHandler<CreateTransaction, CreateTransactionResult>
{
    private readonly WalletDbContext _walletDbContext;
    private readonly IEventDispatcher _eventDispatcher;

    public CreateTransactionCommandHandler(WalletDbContext walletDbContext, IEventDispatcher eventDispatcher)
    {
        _walletDbContext = walletDbContext;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<CreateTransactionResult> Handle(CreateTransaction command, CancellationToken cancellationToken)
    {
        //return error only if ReferenceCode / Id is duplicate.
        //for custom business rule failures, just log.

        Guard.Against.Null(command, nameof(command));

        var transaction = 
            await _walletDbContext.Transactions.AnyAsync(x => x.Id == command.Id || x.ReferenceCode == command.ReferenceCode, cancellationToken);
        if (transaction)
        {
            throw new TransactionAlreadyExistException();
        }
        var paymentCode = WalletExtensions.GetPaymentCode(command.Description);
        var wallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(x => x.PaymentCode == paymentCode, cancellationToken);

        Enums.TransactionStatus status;
        WalletId walletId;

        if (wallet is null)
        {
            status = Enums.TransactionStatus.Unknown;
            walletId = WalletId.Of(Guid.Parse("cba4d432-337c-4824-863c-5af0e7558980"));
        }
        else
        {
            status = Enums.TransactionStatus.Pending;
            walletId = WalletId.Of(wallet.Id.Value);
        }

        var transactionEntity = Models.Transaction.Create(TransactionId.Of(command.Id), walletId, command.Amount,
                command.AccountNumber, command.ReferenceCode, Enums.TransactionType.Topup, status);

        await _walletDbContext.Transactions.AddAsync(transactionEntity, cancellationToken);

        await _walletDbContext.SaveChangesAsync(cancellationToken);

        if (status == Enums.TransactionStatus.Pending)
        {
            await _eventDispatcher.SendAsync(
               new TransactionCreatedDomainEvent(command.Id, wallet.Id, command.ReferenceCode, command.Amount),
               typeof(IInternalCommand));
        }

        return new CreateTransactionResult(command.Id, command.ReferenceCode);   
    }
}



