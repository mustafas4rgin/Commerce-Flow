using System.Data.Common;
using AutoMapper;
using CommerceFlow.Services.Auth.Application.DTOs.Role;
using CommerceFlow.Services.Auth.Application.Interfaces;
using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Services.Auth.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CommerceFlow.Services.Auth.Application.Services;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly IRoleRepository _roleRepository;
    private readonly IValidator<Role> _roleValidator;
    public RoleService(
        IValidator<Role> roleValidator,
        IMapper mapper,
        IRoleRepository roleRepository
    )
    {
        _roleValidator = roleValidator;
        _mapper = mapper;
        _roleRepository = roleRepository;
    }
    public async Task<IEnumerable<Role>> GetRolesAsync(CancellationToken ct = default)
    {
        try
        {
            var roles = _roleRepository.GetRoles(ct);

            if (!roles.Any())
                throw new Exception("There is no role.");
            
            return await roles.ToListAsync(ct);
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message); //TODO: LOGG
        }
    }
    public async Task<Role> GetRoleByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var role = await _roleRepository.GetRoleByIdAsync(id, ct);

            if (role is null)
                throw new Exception("There is no role.");
            
            return role;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<Role> UpdateRoleAsync(int roleId, UpdateRoleDTO dto, CancellationToken ct = default)
    {
        try
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId, ct);

            if (role is null) throw new Exception("There is no role.");

            _mapper.Map(dto, role);

            var validationResult = await _roleValidator.ValidateAsync(role, ct);

            if (!validationResult.IsValid)
                throw new Exception("Validation error."); //TODO: inform properly

            await _roleRepository.UpdateRoleAsync(role, ct);
            await _roleRepository.SaveChangesAsync(ct);

            return role;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<Role> DeleteRoleAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var role = await _roleRepository.GetRoleByIdAsync(id, ct);

            if (role is null) throw new Exception("There is no role.");

            await _roleRepository.DeleteRoleAsync(role, ct);
            await SaveChangesAsync(ct);

            return role;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<Role> CreateRoleAsync(CreateRoleDTO dto, CancellationToken ct = default)
    {
        try
        {
            var createdRole = _mapper.Map<CreateRoleDTO, Role>(dto);

            var validationResult = await _roleValidator.ValidateAsync(createdRole);

            if (!validationResult.IsValid)
                throw new Exception("Validation error."); //TODO: Inform properly.
            
            await _roleRepository.AddRoleAsync(createdRole, ct);
            await SaveChangesAsync(ct);
            //TODO: validasyon
            return createdRole;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task SaveChangesAsync(CancellationToken ct = default)
    => await _roleRepository.SaveChangesAsync(ct);
}