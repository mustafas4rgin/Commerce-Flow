namespace CommerceFlow.Services.Auth.Application.DTOs.Auth;

public sealed class LoginDTO
{
    public string Identifier { get; set; } = null!;

    public string Password { get; set; } = null!;
}