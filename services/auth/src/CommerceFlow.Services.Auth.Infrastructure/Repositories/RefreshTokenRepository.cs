using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFLow.Services.Auth.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CommerceFLow.Services.Auth.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;
    public RefreshTokenRepository(
        AppDbContext context
    )
    {
        _context = context;
    }
public IQueryable<RefreshToken> GetRefreshTokens(CancellationToken ct = default)
=> _context.Set<RefreshToken>().AsNoTracking();
public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token, CancellationToken ct = default)
=> await _context.Set<RefreshToken>().FirstOrDefaultAsync(rt => rt.Token == token, ct);
public void UpdateRefreshToken(RefreshToken refreshToken, CancellationToken ct = default)
=> _context.Set<RefreshToken>().Update(refreshToken);
public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken ct = default)
=> await _context.Set<RefreshToken>().AddAsync(refreshToken, ct);
public void DeleteRefreshToken(RefreshToken refreshToken, CancellationToken ct = default)
=> _context.Set<RefreshToken>().Remove(refreshToken);
public async Task<RefreshToken?> GetRefreshTokenByUserIdAsync(int userId, CancellationToken ct = default)
=> await _context.Set<RefreshToken>().FirstOrDefaultAsync(rt => rt.UserId == userId, ct);
public async Task SaveChangesAsync(CancellationToken ct = default)
=> await _context.SaveChangesAsync(ct);

}