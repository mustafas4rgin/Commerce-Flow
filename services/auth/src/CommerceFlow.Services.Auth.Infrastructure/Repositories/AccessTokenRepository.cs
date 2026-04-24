using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFlow.Services.Auth.Infrastructure.Contexts;
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
    public async Task<AccessToken?> UpdateAccessTokenAsync(AccessToken accessToken, CancellationToken ct = default)
    {
        if (accessToken is null) return null;
        
        var existingAccessToken = await _context.Set<AccessToken>().FindAsync(accessToken.Id, ct);
        if (existingAccessToken is null) return null;

        _context.Set<AccessToken>().Update(accessToken);

        accessToken.UpdatedAt = DateTime.UtcNow;

        return accessToken;
    }
    public async Task<AccessToken?> AddAccessTokenAsync(AccessToken accessToken, CancellationToken ct = default)
    {
        if (accessToken is null) return null;

        await _context.Set<AccessToken>().AddAsync(accessToken, ct);
        return accessToken;
    }
    public void DeleteAccessToken(AccessToken accessToken, CancellationToken ct = default)
    => _context.Set<AccessToken>().Remove(accessToken);
    public async Task<AccessToken?> GetAccessTokenByUserIdAsync(int userId, CancellationToken ct = default)
    => await _context.Set<AccessToken>().AsNoTracking().FirstOrDefaultAsync(at => at.UserId == userId, ct);
    public async Task SaveChangesAsync(CancellationToken ct = default)
    => await _context.SaveChangesAsync(ct);
}