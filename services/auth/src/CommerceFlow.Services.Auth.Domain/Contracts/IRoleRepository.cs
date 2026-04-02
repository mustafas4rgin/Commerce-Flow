using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Domain.Contracts;

public interface IRoleRepository
{
    IQueryable<Role> GetRoles(CancellationToken ct = default);
    Task<Role?> GetRoleByIdAsync(int id, CancellationToken ct = default);
    Task<Role?> UpdateRoleAsync(Role role, CancellationToken ct = default);
    Task<Role?> AddRoleAsync(Role role, CancellationToken ct = default);
    Task<Role?> DeleteRoleAsync(Role role, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}