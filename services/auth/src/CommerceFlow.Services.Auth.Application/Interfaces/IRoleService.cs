using CommerceFlow.Services.Auth.Application.DTOs.Role;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFlow.Shared.Results;

namespace CommerceFlow.Services.Auth.Application.Interfaces;

public interface IRoleService
{
    Task<ServiceResult<List<RoleDTO>>> GetRolesAsync(CancellationToken ct = default);
    Task<ServiceResult<RoleDTO>> GetRoleByIdAsync(int id, CancellationToken ct = default);
    Task<ServiceResult> UpdateRoleAsync(int id, UpdateRoleDTO dto, CancellationToken ct = default);
    Task<ServiceResult> DeleteRoleAsync(int id, CancellationToken ct = default);
    Task<ServiceResult> CreateRoleAsync(CreateRoleDTO dto, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}