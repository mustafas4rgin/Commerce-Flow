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
    public async Task<User?> GetUserByEmailOrUserNameAsync(
    string identifier,
    CancellationToken ct = default)
{
    identifier = identifier.Trim();

    return await _context.Users
        .Include(u => u.Roles)
        .FirstOrDefaultAsync(
            u => !u.IsDeleted &&
                 (
                     EF.Functions.ILike(u.Email, identifier) ||
                     EF.Functions.ILike(u.UserName, identifier)
                 ),
            ct);
}
    public async Task<IEnumerable<User>> GetUsersByLastNameAsync(string lastName, CancellationToken ct = default)
    => await _context.Set<User>().AsNoTracking().Where(u => u.LastName == lastName).ToListAsync(ct);
    public async Task<IEnumerable<User>> GetUsersByFirstNameAsync(string firstName, CancellationToken ct = default)
    => await _context.Set<User>().AsNoTracking().Where(u => u.FirstName == firstName).ToListAsync(ct);
    public async Task<IEnumerable<User>> GetUsersByUserNameAsync(string userName, CancellationToken ct = default)
    => await _context.Set<User>().AsNoTracking().Where(u => u.UserName == userName).ToListAsync(ct);
    public async Task<User?> GetUserByIdAsync(int id, CancellationToken ct = default)
    => await _context.Set<User>().FindAsync(id, ct);
    public async Task<bool> UserExistsByEmailAsync(string email, CancellationToken ct = default)
    => await _context.Set<User>().AnyAsync(u => u.Email == email, ct);
    public async Task<bool> UserExistsByUserNameAsync(string userName, CancellationToken ct = default)
    => await _context.Set<User>().AnyAsync(u => u.UserName == userName, ct);
    public async Task<bool> UserExistsByIdAsync(int id, CancellationToken ct = default)
    => await _context.Set<User>().AnyAsync(u => u.Id == id, ct);
    public async Task<User?> AddUserAsync(User user, CancellationToken ct = default)
    {
        if (user is null) return null;

        await _context.Set<User>().AddAsync(user, ct);

        return user;
    }
    public User? UpdateUser(User user, CancellationToken ct = default)
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
    public async Task<User?> DeleteUserByIdAsync(int id, CancellationToken ct = default)
    {
        var user = await GetUserByIdAsync(id, ct);

        if (user is null) return null;

        user.Delete();

        return user;
    }
    public async Task SaveChangesAsync(CancellationToken ct = default)
    => await _context.SaveChangesAsync(ct);
}