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
    public async Task<ServiceResult<List<UserDTO>>> GetUsersAsync(
        UserFilterDTO dto,
        CancellationToken ct = default)
    {
        try
        {
            var query = _userRepository.GetUsers(ct);

            if (!string.IsNullOrWhiteSpace(dto.UserName)) query = query.Where(u => u.UserName.Contains(dto.UserName));

            if (!string.IsNullOrWhiteSpace(dto.FirstName)) query = query.Where(u => u.FirstName.Contains(dto.FirstName));

            if (!string.IsNullOrWhiteSpace(dto.LastName)) query = query.Where(u => u.LastName.Contains(dto.LastName));

            var users = await query.ToListAsync(ct);

            var userDtos = _mapper.Map<List<UserDTO>>(users);

            if (userDtos.Count == 0) return ServiceResult<List<UserDTO>>.Fail(
                ResultStatus.NotFound,
                "No users found.");

            return ServiceResult<List<UserDTO>>.Ok(userDtos,
            "Users found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting users.");

            return ServiceResult<List<UserDTO>>.Fail(
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
    public async Task<ServiceResult<List<UserDTO>>> GetUsersByUserNameAsync(string userName, CancellationToken ct = default)
    {
        try
        {
            var users = await _userRepository.GetUsersByUserNameAsync(userName, ct);

            if (!users.Any()) return ServiceResult<List<UserDTO>>.Fail(
                ResultStatus.NotFound,
                $"No user found with username: {userName}."
            );

            var usersDto = _mapper.Map<List<UserDTO>>(users);

            return ServiceResult<List<UserDTO>>.Ok(
                usersDto,
                $"Users found with username: {userName}."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting users with username : {userName}.");

            return ServiceResult<List<UserDTO>>.Fail(
                ResultStatus.Error,
                $"An error occurred while getting users wtih username: {userName}."
            );
        }
    }
    public async Task<ServiceResult<List<UserDTO>>> GetUsersByFirstNameAsync(string firstName, CancellationToken ct = default)
    {
        try
        {
            var users = await _userRepository.GetUsersByFirstNameAsync(firstName, ct);

            if (!users.Any()) return ServiceResult<List<UserDTO>>.Fail(
                ResultStatus.NotFound,
                $"There is no user with name {firstName}."
            );

            var userDtos = _mapper.Map<List<UserDTO>>(users);

            return ServiceResult<List<UserDTO>>.Ok(userDtos, "Users found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting users with name : {firstName}.");

            return ServiceResult<List<UserDTO>>.Fail(
                ResultStatus.Error,
                $"An error occurred while getting users with name {firstName}."
            );
        }
    }
    public async Task<ServiceResult<List<UserDTO>>> GetUsersByLastNameAsync(string lastName, CancellationToken ct = default)
    {
        try
        {
            var users = await _userRepository.GetUsersByLastNameAsync(lastName, ct);

            if (!users.Any()) return ServiceResult<List<UserDTO>>.Fail(
                ResultStatus.NotFound,
                $"There is no user with last name {lastName}."
            );

            var userDtos = _mapper.Map<List<UserDTO>>(users);

            return ServiceResult<List<UserDTO>>.Ok(userDtos, "Users found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting user with last name {lastName}.");

            return ServiceResult<List<UserDTO>>.Fail(
                ResultStatus.Error,
                $"An error occurred while getting users with last name {lastName}."
            );
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