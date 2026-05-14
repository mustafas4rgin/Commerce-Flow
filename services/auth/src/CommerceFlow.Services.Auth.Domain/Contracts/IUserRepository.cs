using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Domain.Contracts;

public interface IUserRepository
{
    IQueryable<User> GetUsers(CancellationToken ct = default);
    Task<IEnumerable<User>> GetUsersByUserNameAsync(string userName, CancellationToken ct = default);
    Task<IEnumerable<User>> GetUsersByFirstNameAsync(string firstName, CancellationToken ct = default);
    Task<IEnumerable<User>> GetUsersByLastNameAsync(string lastName, CancellationToken ct = default);
    Task<User?> GetUserByEmailOrUserNameAsync(string identifier, CancellationToken ct = default);
    Task<bool> UserExistsByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> UserExistsByUserNameAsync(string userName, CancellationToken ct = default);
    Task<bool> UserExistsByIdAsync(int id, CancellationToken ct = default);
    Task<User?> GetUserByIdAsync(int id, CancellationToken ct = default);
    Task<User?> AddUserAsync(User user, CancellationToken ct = default);
    User? UpdateUser(User user, CancellationToken ct = default);
    Task<User?> DeleteUserAsync(User user, CancellationToken ct = default);
    Task<User?> DeleteUserByIdAsync(int id, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}