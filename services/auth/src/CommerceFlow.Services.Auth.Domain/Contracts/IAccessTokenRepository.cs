using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Domain.Contracts;

public interface IAccessTokenRepository
{
    IQueryable<AccessToken>GetAccessTokens(CancellationToken ct = default);
    Task<AccessToken?> GetAccessTokenByTokenAsync(string token, CancellationToken ct = default);
    void UpdateAccessToken(AccessToken token, CancellationToken ct = default);
    Task AddAccessTokenAsync(AccessToken token, CancellationToken ct = default);
    void DeleteAccessToken(AccessToken token, CancellationToken ct = default);
    Task<AccessToken?> GetAccessTokenByUserIdAsync(int userId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}