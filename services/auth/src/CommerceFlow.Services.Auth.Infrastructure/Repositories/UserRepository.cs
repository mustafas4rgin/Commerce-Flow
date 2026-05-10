using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFlow.Services.Auth.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CommerceFLow.Services.Auth.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository
    (
        AppDbContext context
    )
    {
        _context = context;
    }
    public IQueryable<User> GetUsers(CancellationToken ct = default)
    => _context.Set<User>().AsNoTracking();
    public async Task<User?> GetUserByIdAsync(int id, CancellationToken ct = default)
    => await _context.Set<User>().FindAsync(id, ct);
    public async Task<User?> AddUserAsync(User user, CancellationToken ct = default)
    {
        if (user is null) return null;

        await _context.Set<User>().AddAsync(user, ct);

        return user;
    }
    public async Task<User?> UpdateUserAsync(User user, CancellationToken ct = default)
    {
        if (user is null || user.Id == default) return null;

        _context.Set<User>().Update(user);

        user.UpdatedAt = DateTime.UtcNow;

        return user;
    }
    public async Task<User?> DeleteUserAsync(User user, CancellationToken ct = default)
    {
        if (user is null) return null;

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;

        return user;
    }
}