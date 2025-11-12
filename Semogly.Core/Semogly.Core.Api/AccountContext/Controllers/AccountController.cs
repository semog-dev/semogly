using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Semogly.Core.Domain.AccountContext.Services.Abstractions;
using CreateCommand = Semogly.Core.Application.AccountContext.UseCases.Create.Command;
using LoginCommand = Semogly.Core.Application.AccountContext.UseCases.Login.Command;

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
            return BadRequest(result.Error);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command
    )
    {
        var result = await mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(result.Error);

        SetRefreshTokenCookie(result.Value.RefreshToken);

        return Ok(result);
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromServices] IJwtService jwtService,
        [FromServices] IRefreshTokenService refreshTokenService
        )
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            return Unauthorized("Refresh token ausente");

        var userId = await refreshTokenService.ValidateAsync(refreshToken);
        if (userId is null)
            return Unauthorized("Refresh token inválido");

        var newAccessToken = jwtService.GenerateToken(userId, "user@email.com");
        var newRefreshToken = await refreshTokenService.GenerateAsync(userId);

        await refreshTokenService.RevokeAsync(refreshToken);
        SetRefreshTokenCookie(newRefreshToken);

        return Ok(new { accessToken = newAccessToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            await _refreshService.RevokeAsync(refreshToken);

        Response.Cookies.Delete("refreshToken");
        return NoContent();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Teste()
    {
        return Ok();
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // obrigar HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}