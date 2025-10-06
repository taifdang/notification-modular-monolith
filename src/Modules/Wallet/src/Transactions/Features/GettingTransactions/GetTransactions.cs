using Ardalis.GuardClauses;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Utils;
using BuildingBlocks.Web;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Data;
using Wallet.Transactions.Dtos;
using Wallet.Transactions.Exceptions;


namespace Wallet.Transactions.Features.GettingTransactions;
public record GetTransactions : IQuery<GetTransactionsResult>;
public record GetTransactionsResult(IEnumerable<TransactionDto> TransactionDto);
public record GetTransactionstResponseDto(IEnumerable<TransactionDto> TransactionDto);

[ApiController]
[Route("api/transaction")]
public class GetTransactionsEndpoint : ControllerBase
{
    private readonly IMediator _mediator;
    public GetTransactionsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Result<GetTransactionstResponseDto>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTransactions(), cancellationToken);
        return result is null
            ? Result<GetTransactionstResponseDto>.Failure("pleast try again")
            : Result<GetTransactionstResponseDto>.Success(result.Adapt<GetTransactionstResponseDto>());
    }
}

internal class GetAvailableTransactionsHandler : IQueryHandler<GetTransactions, GetTransactionsResult>
{
    private readonly WalletDbContext _walletDbContext;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IMapper _mapper;

    public GetAvailableTransactionsHandler(WalletDbContext walletDbContext, IMapper mapper, ICurrentUserProvider currentUserProvider)
    {
        _walletDbContext = walletDbContext;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<GetTransactionsResult> Handle(GetTransactions request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var userId = _currentUserProvider.GetCurrentUserId();
        if (userId == null || userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }
        //LINQ query syntax / method syntax
        var transactions = await _walletDbContext.Transactions
          .Where(t => _walletDbContext.Wallets
            .Any(w => w.Id == t.WalletId && w.UserId == userId))
          .OrderByDescending(t => t.CreatedAt)
          .ToListAsync();

        if (!transactions.Any())
        {
            throw new TransactionNotFoundException();
        }

        var transactionDtos = _mapper.Map<IEnumerable<TransactionDto>>(transactions);

        return new GetTransactionsResult(transactionDtos);
    }
}



