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
}