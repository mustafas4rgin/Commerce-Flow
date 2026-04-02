using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFLow.Services.Auth.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CommerceFLow.Services.Auth.Infrastructure.Repositories;

public class AccessTokenRepository : IAccessTokenRepository
{
    private readonly AppDbContext _context;
    public AccessTokenRepository(
        AppDbContext context
    )
    {
        _context = context;
    }
    public IQueryable<AccessToken> GetAccessTokens(CancellationToken ct = default)
    => _context.Set<AccessToken>().AsNoTracking();
    public async Task<AccessToken?> GetAccessTokenByTokenAsync(string token, CancellationToken ct = default)
    => await _context.Set<AccessToken>().AsNoTracking().FirstOrDefaultAsync(at => at.Token == token, ct);
    public void UpdateAccessToken(AccessToken accessToken, CancellationToken ct = default)
    => _context.Set<AccessToken>().Update(accessToken);
    public async Task AddAccessTokenAsync(AccessToken accessToken, CancellationToken ct = default)
    => await _context.Set<AccessToken>().AddAsync(accessToken, ct);
    public void DeleteAccessToken(AccessToken accessToken, CancellationToken ct = default)
    => _context.Set<AccessToken>().Remove(accessToken);
    public async Task<AccessToken?> GetAccessTokenByUserIdAsync(int userId, CancellationToken ct = default)
    => await _context.Set<AccessToken>().AsNoTracking().FirstOrDefaultAsync(at => at.UserId == userId, ct);
    public async Task SaveChangesAsync(CancellationToken ct = default)
    => await _context.SaveChangesAsync(ct);
}