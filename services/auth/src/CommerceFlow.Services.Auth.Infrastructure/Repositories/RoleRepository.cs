using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFLow.Services.Auth.Infrastructure.Contexts;
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
    public IQueryable<Role> GetRoles(CancellationToken ct = default)
    => _context.Set<Role>().AsNoTracking();
    public async Task<Role?> GetRoleByIdAsync(int id, CancellationToken ct = default)
    => await _context.Set<Role>().FindAsync(id, ct);
    public async Task<Role?> AddRoleAsync(Role role, CancellationToken ct = default)
    {
        if (role is null) return null;

        await _context.AddAsync(role, ct);

        return role;
    }
    public async Task<Role?> UpdateRoleAsync(Role role, CancellationToken ct = default)
    {
        if (role is null || role.Id == default) return null;

        var existingRole = await _context.Set<Role>().FindAsync(role.Id);
        if (existingRole is null) return null;

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
}