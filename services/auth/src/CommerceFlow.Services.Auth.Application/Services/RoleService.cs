
using AutoMapper;
using CommerceFlow.Services.Auth.Application.DTOs.Role;
using CommerceFlow.Services.Auth.Application.Interfaces;
using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFlow.Shared.Results;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommerceFlow.Services.Auth.Application.Services;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly IRoleRepository _roleRepository;
    private readonly IValidator<Role> _roleValidator;
    private readonly ILogger<RoleService> _logger;
    public RoleService(
        IValidator<Role> roleValidator,
        IMapper mapper,
        IRoleRepository roleRepository,
        ILogger<RoleService> logger
    )
    {
        _logger = logger;
        _roleValidator = roleValidator;
        _mapper = mapper;
        _roleRepository = roleRepository;
    }
    public async Task<ServiceResult<List<RoleDTO>>> GetRolesAsync(CancellationToken ct = default)
    {
        try
        {
            var roles = await _roleRepository.GetRoles(ct).ToListAsync(ct);

            var roleDtos = _mapper.Map<List<RoleDTO>>(roles);

            var message = roleDtos.Any()
                ? "Roles listed successfully."
                : "No roles found.";

            return ServiceResult<List<RoleDTO>>.Ok(roleDtos, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting roles.");

            return ServiceResult<List<RoleDTO>>.Fail(
                ResultStatus.Error,
                "An error occurred while getting roles."
            );
        }
    }
    public async Task<ServiceResult<RoleDTO>> GetRoleByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var role = await _roleRepository.GetRoleByIdAsync(id, ct);

            if (role is null)
                return ServiceResult<RoleDTO>.Fail(ResultStatus.NotFound, "There is no role with that id.");

            var dto = _mapper.Map<RoleDTO>(role);

            return ServiceResult<RoleDTO>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting role. RoleID: {RoleID}", id);

            return ServiceResult<RoleDTO>.Fail(
                ResultStatus.Error,
                "An error occurred while getting role."
            );
        }
    }
    public async Task<ServiceResult> UpdateRoleByIdAsync(int roleId, UpdateRoleDTO dto, CancellationToken ct = default)
    {
        try
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId, ct);

            if (role is null) return ServiceResult.Fail(ResultStatus.NotFound, $"There is no role with ID : {roleId}");

            _mapper.Map(dto, role);

            var validationResult = await _roleValidator.ValidateAsync(role, ct);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return ServiceResult.Fail(
                    ResultStatus.ValidationError,
                    "Validation error.",
                    errors
                );
            }

            if (await _roleRepository.RoleNameExistsAsync(role.Name, roleId, ct))
                return ServiceResult.Fail(ResultStatus.Conflict, "Role with that name already exists.");

            await _roleRepository.UpdateRoleAsync(role, ct);
            await _roleRepository.SaveChangesAsync(ct);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating role. RoleName: {RoleName}", dto.Name);

            return ServiceResult.Fail(
                ResultStatus.Error,
                "An error occurred while updating role."
            );
        }
    }
    public async Task<ServiceResult> DeleteRoleByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var role = await _roleRepository.GetRoleByIdAsync(id, ct);

            if (role is null) return ServiceResult.Fail(ResultStatus.NotFound, "There is no role with that id.");

            await _roleRepository.DeleteRoleAsync(role, ct);
            await SaveChangesAsync(ct);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting role. Role ID: {RoleId}", id);

            return ServiceResult.Fail(
                ResultStatus.Error,
                "An error occurred while deleting role."
            );
        }
    }
    public async Task<ServiceResult> CreateRoleAsync(CreateRoleDTO dto, CancellationToken ct = default)
    {
        try
        {
            var createdRole = _mapper.Map<CreateRoleDTO, Role>(dto);

            var validationResult = await _roleValidator.ValidateAsync(createdRole, ct);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return ServiceResult.Fail(
                    ResultStatus.ValidationError,
                    "Validation error.",
                    errors
                );
            }

            var existingRole = await _roleRepository.GetRoleByNameAsync(createdRole.Name, ct);

            if (existingRole is not null) return ServiceResult.Fail(ResultStatus.Conflict, "That role already exists.");

            await _roleRepository.AddRoleAsync(createdRole, ct);
            await SaveChangesAsync(ct);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating role. RoleName: {RoleName}", dto.Name);

            return ServiceResult.Fail(
                ResultStatus.Error,
                "An error occurred while creating role."
            );
        }
    }
    public async Task SaveChangesAsync(CancellationToken ct = default)
    => await _roleRepository.SaveChangesAsync(ct);
}