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
using Wallet.Wallets.Dtos;
using Wallet.Wallets.Exceptions;


namespace Wallet.Wallets.Features.GettingMyWallet;

public record GetMyWallet : IQuery<GetMyWalletResult>;
public record GetMyWalletResult(WalletDto WalletDto);
public record GetMyWalletResponseDto(WalletDto WalletDto);

[ApiController]
public class GetMyWalletEndpoint : ControllerBase
{
    private readonly IMediator _mediator;
    public GetMyWalletEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("wallet/me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Result<GetMyWalletResponseDto>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyWallet(), cancellationToken);
        return result is null
            ? Result<GetMyWalletResponseDto>.Failure("Wallet not found")
            : Result<GetMyWalletResponseDto>.Success(result.Adapt<GetMyWalletResponseDto>());
    }
}

internal class GetMyWalletHandler : IQueryHandler<GetMyWallet, GetMyWalletResult>
{
    private readonly WalletDbContext _walletDbContext;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IMapper _mapper;

    public GetMyWalletHandler(WalletDbContext walletDbContext, IMapper mapper, ICurrentUserProvider currentUserProvider)
    {
        _walletDbContext = walletDbContext;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<GetMyWalletResult> Handle(GetMyWallet request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var userId = _currentUserProvider.GetCurrentUserId();
        if (userId == null || userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var wallet = await _walletDbContext.Wallets
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (wallet is null)
            throw new WalletNotFoundException();

        var walletDto = _mapper.Map<WalletDto>(wallet);

        return new GetMyWalletResult(walletDto);
    }
}