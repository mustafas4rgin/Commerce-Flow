using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFLow.Services.Auth.Infrastructure.Contexts;

namespace CommerceFLow.Services.Auth.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    public AuthRepository(
        AppDbContext context
    )
    {
        _context = context;
    }
    public async Task RegisterUserAsync(User user, CancellationToken ct = default)
    => await _context.Set<User>().AddAsync(user, ct);
    public async Task SaveChangesAsync(CancellationToken ct = default)
    => await _context.SaveChangesAsync(ct);
    
}