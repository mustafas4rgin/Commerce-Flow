using CommerceFlow.Services.Auth.Application.DTOs.Role;
using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Application.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<Role>> GetRolesAsync(CancellationToken ct = default);
    Task<Role> GetRoleByIdAsync(int id, CancellationToken ct = default);
    Task<Role> UpdateRoleAsync(int id, UpdateRoleDTO dto, CancellationToken ct = default);
    Task<Role> DeleteRoleAsync(int id, CancellationToken ct = default);
    Task<Role> CreateRoleAsync(CreateRoleDTO dto, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}