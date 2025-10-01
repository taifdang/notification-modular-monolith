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


namespace Wallet.Transactions.Features.GettingAvailableTransactions;
public record GetAvailableTransactions : IQuery<GetAvailableTransactionsResult>;
public record GetAvailableTransactionsResult(IEnumerable<TransactionDto> TransactionDto);
public record GetAvailableTransactionstResponseDto(IEnumerable<TransactionDto> TransactionDto);

[ApiController]
[Route("api/transaction")]
public class GetAvailableTransactionsEndpoint : ControllerBase
{
    private readonly IMediator _mediator;
    public GetAvailableTransactionsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Result<GetAvailableTransactionstResponseDto>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAvailableTransactions(), cancellationToken);
        return result is null
            ? Result<GetAvailableTransactionstResponseDto>.Failure("pleast try again")
            : Result<GetAvailableTransactionstResponseDto>.Success(result.Adapt<GetAvailableTransactionstResponseDto>());
    }
}

internal class GetAvailableTransactionsHandler : IQueryHandler<GetAvailableTransactions, GetAvailableTransactionsResult>
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

    public async Task<GetAvailableTransactionsResult> Handle(GetAvailableTransactions request, CancellationToken cancellationToken)
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

        return new GetAvailableTransactionsResult(transactionDtos);
    }
}



