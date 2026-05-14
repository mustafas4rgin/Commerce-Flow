namespace CommerceFlow.Services.Auth.Application.DTOs.Auth;

public sealed class AuthResponseDTO
{
    public string AccessToken { get; set; } = null!;
    public DateTime AccessTokenExpiration { get; set; }
}