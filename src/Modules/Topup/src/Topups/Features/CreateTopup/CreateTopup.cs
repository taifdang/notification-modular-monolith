
using BuildingBlocks.Contracts;
using BuildingBlocks.Core;
using BuildingBlocks.Core.CQRS;
using BuildingBlocks.Core.Event;
using BuildingBlocks.Utils;
using FluentValidation;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Topup.Data;
using Topup.Topups.Exceptions;
using Topup.Topups.ValueObjects;

namespace Topup.Topups.Features.CreateTopup;

public record CreateTopup(string Gateway,string TransactionDate,string AccountNumber,string? SubAccount,string? Code,
    string Content,string TransferType,string Description,decimal TransferAmount,int Accumulated,int Id)
    : ICommand<CreateTopupResult>
{
    public Guid TopupId { get; init; } = NewId.NextGuid();
};

public record CreateTopupResult(int TransactionId,string AccountNumber,string TransferType,decimal TransferAmount);

public record TopupCreatedDomainEvent(int Id,string CreateByName,decimal TransferAmount): IHaveIntegrationEvent;

[ApiController]
public class CreateTopupEndpoint : ControllerBase
{
    private readonly IMediator _mediator;
    public CreateTopupEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-topup")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Result<CreateTopupResult>> TopupRequest(
        CreateTopup command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result is null)
            return Result<CreateTopupResult>.Failure();
        return Result<CreateTopupResult>.Success(result);
    }
}

public class CreateTopupValidator : AbstractValidator<CreateTopup> 
{ 
    public CreateTopupValidator()
    {
        RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Please enter field AccountNumber")
                    .Matches(@"^[0-9]+$").WithMessage("A valid AccountNumber required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter the Description");
        RuleFor(x => x.TransferAmount).GreaterThan(0).WithMessage("Please enter the TransferAmount");
    }
}

public class CreateTopupHandler : ICommandHandler<CreateTopup, CreateTopupResult>
{
    private readonly TopupDbContext _topupDbContext;
    private readonly IMapper _mapper;
    private readonly IEventDispatcher _dispatcher;

    public CreateTopupHandler(
        TopupDbContext topupDbContext, 
        IMapper mapper,
        IEventDispatcher dispatcher)
    {
        _topupDbContext = topupDbContext;
        _mapper = mapper;
        _dispatcher = dispatcher;
    }

    public async Task<CreateTopupResult> Handle(
        CreateTopup request,
        CancellationToken cancellationToken)
    {
        var topup = await _topupDbContext.Topups.SingleOrDefaultAsync(
            x => x.TransactionId.Value == request.Id,
            cancellationToken);

        if (topup is not null)
        {
            throw new TopupAlreadyExistException();
        }

        //internal
        var username = nameof(Enums.TopupStatus.Unknown);
        Enums.TopupStatus status = Enums.TopupStatus.Unknown;
        var user = request.Description.StartsWith("NAPTIEN");

        if (user)
        {
            username = request.Description.Split("NAPTIEN ")[1].ToLower();
            status = Enums.TopupStatus.InProcess;
        }
 
        //no domainEvent
        var topupEntity = Models.Topup.Create(TopupId.Of(request.TopupId),TransactionId.Of(request.Id),
            TransferAmount.Of(request.TransferAmount),CreateByName.Of(username),status);

        await _dispatcher.SendAsync(new TopupCreated(topupEntity.TransactionId),cancellationToken: cancellationToken);

        await _topupDbContext.AddAsync(topupEntity, cancellationToken);

        await _topupDbContext.SaveChangesAsync(cancellationToken);

        return new CreateTopupResult(request.Id,request.AccountNumber,request.TransferType,request.TransferAmount);
    }
}

