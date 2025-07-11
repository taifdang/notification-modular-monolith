using Hookpay.Modules.Topups.Core.Topups.Features;
using Hookpay.Shared.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hookpay.Modules.Topups.Api.Controllers
{
    [ApiController]
    [Route("api/webhook/receive")]
    public class TopupsController:ControllerBase
    {
        private readonly IMediator _mediatr;
        public TopupsController(IMediator mediatr) { _mediatr = mediatr; }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<Result<object>> CreateTopupEvent(CreateTopupCommand command,CancellationToken cancellationToken)
        {
            var result = await _mediatr.Send(command, cancellationToken); 
            if(result is null) return Result<object>.Failure();           
            return Result<object>.Success(result);

        }
    }
}
