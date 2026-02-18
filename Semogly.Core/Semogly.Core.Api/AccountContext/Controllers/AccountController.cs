using MediatR;
using Microsoft.AspNetCore.Mvc;
using Semogly.Core.Api.SharedContext.Common;
using CreateCommand = Semogly.Core.Application.AuthContext.UseCases.CreateAccount.Command;
using LoginCommand = Semogly.Core.Application.AuthContext.UseCases.Login.Command;
using RefreshCommand = Semogly.Core.Application.AccountContext.UseCases.Refresh.Command;
using MeCommand = Semogly.Core.Application.AccountContext.UseCases.Me.Command;
using LogoutCommand = Semogly.Core.Application.AccountContext.UseCases.Logout.Command;
using Microsoft.AspNetCore.Authorization;

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

        return NoContent();
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var command = new RefreshCommand();
        var result = await mediator.Send(command);  

        if (result.IsFailure)
            return Unauthorized();

        return NoContent();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutCommand();
        var result = await mediator.Send(command);  

        if (result.IsFailure)
            return Unauthorized();

        return NoContent();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var command = new MeCommand();
        var result = await mediator.Send(command);  

        if (result.IsFailure)
            return Unauthorized();

        return Ok(result.Value);
    }
}