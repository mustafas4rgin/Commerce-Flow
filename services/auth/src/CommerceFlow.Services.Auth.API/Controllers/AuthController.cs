using CommerceFlow.Services.Auth.Application.DTOs.Auth;
using CommerceFlow.Services.Auth.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommerceFlow.Services.Auth.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginDTO dto,
        CancellationToken ct = default)
    {
        var result = await _authService.LoginAsync(dto, ct);

        return ToActionResult(result);
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterDTO dto,
        CancellationToken ct = default
    )
    {
        var result = await _authService.RegisterAsync(dto, ct);

        return ToActionResult(result);
    }
    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync(
        CancellationToken ct = default
    )
    {
        var userId = CurrentUserId;

        if (userId is null)
            return Unauthorized("Invalid token.");
        
        var result = await _authService.GetMeAsync(userId.Value, ct);

        return ToActionResult(result);
    }
}