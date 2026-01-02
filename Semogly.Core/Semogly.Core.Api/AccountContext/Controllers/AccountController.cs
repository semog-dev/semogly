using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Semogly.Core.Api.SharedContext.Common;
using Semogly.Core.Application.AccountContext.UseCases.Login;
using CreateCommand = Semogly.Core.Application.AccountContext.UseCases.Create.Command;
using LoginCommand = Semogly.Core.Application.AccountContext.UseCases.Login.Command;
using RefreshCommand = Semogly.Core.Application.AccountContext.UseCases.Refresh.Command;

namespace Semogly.Core.Api.AccountContext.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCommand command
    )
    {
        var result = await mediator.Send(command);        

        if (result.IsFailure)
            return StatusCode(result.Error.ToStatusCode(), result.Error);

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command
    )
    {
        var result = await mediator.Send(command);        

        if (result.IsFailure)
            return StatusCode(result.Error.ToStatusCode(), result.Error);        

        return Ok(result.Value);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {

        var command = new RefreshCommand();
        var result = await mediator.Send(command);  

        if (result.IsFailure)
            return Unauthorized();

        return Ok(result.Value);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Teste()
    {
        return Ok();
    }
}