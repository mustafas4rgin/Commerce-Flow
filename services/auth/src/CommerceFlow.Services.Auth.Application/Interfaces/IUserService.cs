using CommerceFlow.Services.Auth.Application.DTOs.User;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFlow.Shared.Results;

namespace CommerceFlow.Services.Auth.Application.Interfaces;

public interface IUserService
{
    Task<ServiceResult<List<UserDTO>>> GetUsersAsync(CancellationToken ct = default);
    Task<ServiceResult<UserDTO>> GetUserByIdAsync(CancellationToken ct = default);
    Task<ServiceResult<UserDTO>> GetUserByFirstNameAsync(string firstName, CancellationToken ct = default);
    Task<ServiceResult<UserDTO>> GetUserByLastNameAsync(string firstName, CancellationToken ct = default);
    Task<ServiceResult> CreateUserAsync(CreateUserDTO dto, CancellationToken ct = default);
    Task<ServiceResult> UpdateUserByIdAsync(int id, CancellationToken ct = default);
    Task<ServiceResult> DeleteUserByIdAsync(int id, CancellationToken ct = default);
}