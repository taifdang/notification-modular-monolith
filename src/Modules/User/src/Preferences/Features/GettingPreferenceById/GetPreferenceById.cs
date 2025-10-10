namespace User.Preferences.Features.GettingPreferenceById;

using Ardalis.GuardClauses;
using BuildingBlocks.Core.CQRS;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using User.Data;
using User.Preferences.Dtos;
using User.Preferences.Exceptions;

public record GetPreferenceById(Guid Id) : IQuery<GetPreferenceByIdResult>;
public record GetPreferenceByIdResult(PreferenceDto PreferenceDto);
public record GetPreferenceByIdResponse(PreferenceDto PreferenceDto);

[ApiController]
[Route("")]
public class GetPreferenceByIdEndpoint : ControllerBase
{
    [HttpGet("api/preferences/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPreferenceById(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPreferenceById(id), cancellationToken);
        var response = result.Adapt<GetPreferenceByIdResponse>();
        return Ok(result);
    }
}
public class GetPreferenceByIdValidator : AbstractValidator<GetPreferenceById>
{
    public GetPreferenceByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
public class GetPreferenceByIdHandler : IRequestHandler<GetPreferenceById, GetPreferenceByIdResult>
{
    private readonly IMapper _mapper;
    private readonly UserDbContext _userDbContext;

    public GetPreferenceByIdHandler(IMapper mapper, UserDbContext userDbContext)
    {
        _mapper = mapper;
        _userDbContext = userDbContext;
    }
    public async Task<GetPreferenceByIdResult> Handle(GetPreferenceById query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(query));

        //var preference = await _userDbContext.Preferences
        //    .Where(x => x.UserId.Value == query.Id && !x.IsDeleted)
        //    .ToListAsync();

        var preference = await _userDbContext.Preferences
            .Where(x => x.UserId.Value == query.Id && !x.IsDeleted)
            .GroupBy(x => x.UserId.Value)
            .Select(g => new PreferenceDto(
                g.Key,
                g.Select(p => new ChannelPreference(p.Channel, p.IsOptOut)).ToList()
            ))
            .FirstOrDefaultAsync();

        if (preference is null)
        {
            throw new PreferenceNotFoundException();
        }

        //var preferenceDto = _mapper.Map<PreferenceDto>(preference);

        return new GetPreferenceByIdResult(preference);
    }
}
