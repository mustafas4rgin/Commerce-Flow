using CommerceFlow.Services.Auth.Application.DTOs.User;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFlow.Shared.Results;

namespace CommerceFlow.Services.Auth.Application.Interfaces;

public interface IUserService
{
    Task<ServiceResult<List<UserDTO>>> GetUsersAsync(UserFilterDTO dto, CancellationToken ct = default);
    Task<ServiceResult<UserDTO>> GetUserByIdAsync(int userId, CancellationToken ct = default);
    Task<ServiceResult<List<UserDTO>>> GetUsersByFirstNameAsync(string firstName, CancellationToken ct = default);
    Task<ServiceResult<List<UserDTO>>> GetUsersByLastNameAsync(string lastName, CancellationToken ct = default);
    Task<ServiceResult<List<UserDTO>>> GetUsersByUserNameAsync(string userName, CancellationToken ct = default);
    Task<ServiceResult> CreateUserAsync(CreateUserDTO dto, CancellationToken ct = default);
    Task<ServiceResult> UpdateUserByIdAsync(int id, UpdateUserDTO dto, CancellationToken ct = default);
    Task<ServiceResult> DeleteUserByIdAsync(int id, CancellationToken ct = default);
}