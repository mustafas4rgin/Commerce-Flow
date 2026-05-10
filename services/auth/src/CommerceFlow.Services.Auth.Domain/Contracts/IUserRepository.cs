using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Domain.Contracts;

public interface IUserRepository
{
    IQueryable<User> GetUsers(CancellationToken ct = default);
    Task<User?> GetUserByIdAsync(int id, CancellationToken ct = default);
    Task<User?> AddUserAsync(User user, CancellationToken ct = default);
    Task<User?> UpdateUserAsync(User user, CancellationToken ct = default);
    Task<User?> DeleteUserAsync(User user, CancellationToken ct = default);
}