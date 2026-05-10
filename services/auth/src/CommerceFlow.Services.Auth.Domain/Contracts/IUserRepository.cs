using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Domain.Contracts;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsers(CancellationToken ct = default);
    Task<User?> GetUserByIdAsync(int id, CancellationToken ct = default);
    Task<User?> AddUserAsync(Role role, CancellationToken ct = default);
    Task<User?> UpdateUserAsync(Role role, CancellationToken ct = default);
    Task<User?> DeleteUserAsync(Role role, CancellationToken ct = default);
}