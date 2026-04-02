using CommerceFlow.Shared.BuildingBlocks;

namespace CommerceFlow.Services.Auth.Domain.Entities;

public sealed class AccessToken : EntityBase
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public int UserId { get; set; }
    //nav prop
    public User User { get; set; } = null!;
}