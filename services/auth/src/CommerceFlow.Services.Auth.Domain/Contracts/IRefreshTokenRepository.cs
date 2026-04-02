using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Domain.Contracts;

public interface IRefreshTokenRepository
{
    IQueryable<RefreshToken>GetRefreshTokens(CancellationToken ct = default);
    Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token, CancellationToken ct = default);
    void UpdateRefreshToken(RefreshToken token, CancellationToken ct = default);
    Task AddRefreshTokenAsync(RefreshToken token, CancellationToken ct = default);
    void DeleteRefreshToken(RefreshToken token, CancellationToken ct = default);
    Task<RefreshToken?> GetRefreshTokenByUserIdAsync(int userId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}