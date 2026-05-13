using AutoMapper;
using CommerceFlow.Services.Auth.Application.DTOs.User;
using CommerceFlow.Services.Auth.Application.Interfaces;
using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFlow.Shared.Results;
using CommerceFlow.Shared.Validation.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommerceFlow.Services.Auth.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<User> _validator;
    private readonly ILogger<UserService> _logger;
    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        IValidator<User> validator,
        ILogger<UserService> logger
    )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }
    public async Task<ServiceResult<PagedResult<UserDTO>>> GetUsersAsync(
        UserFilterDTO filter,
        CancellationToken ct = default)
    {
        try
        {
            if (filter.PageNumber < 1)
                filter.PageNumber = 1;

            if (filter.PageSize < 1)
                filter.PageSize = 10;

            if (filter.PageSize > 50)
                filter.PageSize = 50;

            var query = _userRepository.GetUsers(ct);

            if (!string.IsNullOrWhiteSpace(filter.UserName)) query = query.Where(u => u.UserName.Contains(filter.UserName));

            if (!string.IsNullOrWhiteSpace(filter.FirstName)) query = query.Where(u => u.FirstName.Contains(filter.FirstName));

            if (!string.IsNullOrWhiteSpace(filter.LastName)) query = query.Where(u => u.LastName.Contains(filter.LastName));

            var totalCount = await query.CountAsync(ct);

            var users = await query
                .OrderBy(u => u.Id)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(ct);

            var userDtos = _mapper.Map<List<UserDTO>>(users);

            var pagedResult = new PagedResult<UserDTO>
            {
                Items = userDtos,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount

            };

            return ServiceResult<PagedResult<UserDTO>>.Ok(
                pagedResult,
                "Users found."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting users.");

            return ServiceResult<PagedResult<UserDTO>>.Fail(
                ResultStatus.Error,
                "An error occurred while getting users.");
        }
    }
    public async Task<ServiceResult<UserDTO>> GetUserByIdAsync(int userId, CancellationToken ct = default)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId, ct);

            if (user is null) return ServiceResult<UserDTO>.Fail(
                ResultStatus.NotFound,
                $"No user found with ID : {userId}"
            );

            var userDto = _mapper.Map<UserDTO>(user);

            return ServiceResult<UserDTO>.Ok(userDto, "User found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting user with ID : {userId}");

            return ServiceResult<UserDTO>.Fail(
                ResultStatus.Error,
                $"An error occurred while getting user with ID : {userId}");
        }
    }
    public async Task<ServiceResult> CreateUserAsync(CreateUserDTO dto, CancellationToken ct = default)
    {
        try
        {
            var createdUser = _mapper.Map<CreateUserDTO, User>(dto);

            var validationResult = await _validator.ValidateAsync(createdUser);

            if (!validationResult.IsValid) return ValidationResultExtensions.ToServiceResult(validationResult);

            var userExistsWithEmail = await _userRepository.UserExistsByEmailAsync(createdUser.Email, ct);

            if (userExistsWithEmail) return ServiceResult.Fail(
                ResultStatus.Conflict,
                $"There is a user with {createdUser.Email} email."
            );

            var userExistsWithUserName = await _userRepository.UserExistsByUserNameAsync(createdUser.UserName, ct);

            if (userExistsWithUserName) return ServiceResult.Fail(
                ResultStatus.Conflict,
                $"There is a user with {createdUser.UserName} username."
            );

            await _userRepository.AddUserAsync(createdUser, ct);
            await _userRepository.SaveChangesAsync(ct);

            return ServiceResult.Ok("User created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while creating user.");

            return ServiceResult.Fail(
                ResultStatus.Error,
                "An error occurred while creating user."
            );
        }
    }
    public async Task<ServiceResult> UpdateUserByIdAsync(int id, UpdateUserDTO dto, CancellationToken ct = default)
    {
        try
        {
            var updatingUser = await _userRepository.GetUserByIdAsync(id, ct);

            if (updatingUser is null)
                return ServiceResult.Fail(
                    ResultStatus.NotFound,
                    $"No user found with ID : {id}"
                );

            _mapper.Map(dto, updatingUser);

            var validationResult = await _validator.ValidateAsync(updatingUser, ct);

            if (!validationResult.IsValid)
                return ValidationResultExtensions.ToServiceResult(validationResult);

            await _userRepository.SaveChangesAsync(ct);

            return ServiceResult.Ok($"User updated with username {updatingUser.UserName}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating user with id: {UserId}", id);

            return ServiceResult.Fail(
                ResultStatus.Error,
                $"An error occurred while updating user with id : {id}."
            );
        }
    }
    public async Task<ServiceResult> DeleteUserByIdAsync(int id, CancellationToken ct = default)
    {
        try
        {
            var deletedUser = await _userRepository.DeleteUserByIdAsync(id, ct);

            if (deletedUser is null)
                return ServiceResult.Fail(
                    ResultStatus.NotFound,
                    $"No user found with ID : {id}."
                );

            await _userRepository.SaveChangesAsync(ct);

            return ServiceResult.Ok($"User with {id} id deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting user with ID: {UserId}", id);

            return ServiceResult.Fail(
                ResultStatus.Error,
                $"An error occurred while deleting user with ID : {id}."
            );
        }
    }
}