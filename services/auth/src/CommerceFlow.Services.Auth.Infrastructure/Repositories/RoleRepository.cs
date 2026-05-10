using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFlow.Services.Auth.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CommerceFLow.Services.Auth.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;
    public RoleRepository(
        AppDbContext context
    )
    {
        _context = context;
    }
    public async Task<bool> RoleExistsAsync(int id) 
    => await _context.Set<Role>().AnyAsync(r => r.Id == id);
    public async Task<bool> RoleNameExistsAsync(
    string name,
    int exceptRoleId,
    CancellationToken ct = default)
    => await _context.Set<Role>()
                .AnyAsync(r => r.Id != exceptRoleId && r.Name == name, ct);
    public IQueryable<Role> GetRoles(CancellationToken ct = default)
    => _context.Set<Role>().AsNoTracking();
    public async Task<Role?> GetRoleByIdAsync(int id, CancellationToken ct = default)
    => await _context.Set<Role>().FindAsync(id, ct);
    public async Task<Role?> GetRoleByNameAsync(string name, CancellationToken ct = default)
    => await _context.Set<Role>().FirstOrDefaultAsync(r => r.Name == name, ct);
    public async Task<Role?> AddRoleAsync(Role role, CancellationToken ct = default)
    {
        if (role is null) return null;

        await _context.AddAsync(role, ct);

        return role;
    }
    public async Task<Role?> UpdateRoleAsync(Role role, CancellationToken ct = default)
    {
        if (role is null || role.Id == default) return null;

        _context.Set<Role>().Update(role);

        role.UpdatedAt = DateTime.UtcNow;

        return role;
    }
    public async Task<Role?> DeleteRoleAsync(Role role, CancellationToken ct = default)
    {
        if (role is null) return null;

        role.IsDeleted = true;
        role.DeletedAt = DateTime.UtcNow;

        return role;
    }
    public async Task SaveChangesAsync(CancellationToken ct = default)
    => await _context.SaveChangesAsync(ct);
}