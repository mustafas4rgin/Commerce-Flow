using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Domain.Contracts;

public interface IRoleRepository
{
    IQueryable<Role> GetRoles(CancellationToken ct = default);
    Task<Role?> GetRoleByIdAsync(int id, CancellationToken ct = default);
    Role? UpdateRole(Role role, CancellationToken ct = default);
    Task<Role?> AddRoleAsync(Role role, CancellationToken ct = default);
    Role? DeleteRole(Role role, CancellationToken ct = default);
    Task<Role?> GetRoleByNameAsync(string name, CancellationToken ct = default);
    Task<bool> RoleExistsAsync(int id);
    Task<bool> RoleNameExistsAsync(
    string name,
    int exceptRoleId,
    CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}