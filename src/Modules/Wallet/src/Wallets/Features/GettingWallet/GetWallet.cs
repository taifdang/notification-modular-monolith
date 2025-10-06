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


namespace Wallet.Wallets.Features.GettingWallet;

public record GetWallet : IQuery<GetWalletResult>;
public record GetWalletResult(WalletDto WalletDto);
public record GetWalletResponseDto(WalletDto WalletDto);

[ApiController]
[Route("api/wallet")]
public class GetWalletEndpoint : ControllerBase
{
    private readonly IMediator _mediator;
    public GetWalletEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Result<GetWalletResponseDto>> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWallet(), cancellationToken);
        return result is null
            ? Result<GetWalletResponseDto>.Failure("Wallet not found")
            : Result<GetWalletResponseDto>.Success(result.Adapt<GetWalletResponseDto>());
    }
}

internal class GetWalletHandler : IQueryHandler<GetWallet, GetWalletResult>
{
    private readonly WalletDbContext _walletDbContext;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IMapper _mapper;

    public GetWalletHandler(WalletDbContext walletDbContext, IMapper mapper, ICurrentUserProvider currentUserProvider)
    {
        _walletDbContext = walletDbContext;
        _mapper = mapper;
        _currentUserProvider = currentUserProvider;
    }

    public async Task<GetWalletResult> Handle(GetWallet request, CancellationToken cancellationToken)
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

        return new GetWalletResult(walletDto);
    }
}