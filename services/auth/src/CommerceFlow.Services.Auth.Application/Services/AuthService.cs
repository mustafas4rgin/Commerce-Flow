using AutoMapper;
using CommerceFlow.Services.Auth.Application.DTOs.Auth;
using CommerceFlow.Services.Auth.Application.Interfaces;
using CommerceFlow.Services.Auth.Application.Options;
using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Services.Auth.Domain.Entities;
using CommerceFlow.Shared.Constants;
using CommerceFlow.Shared.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommerceFlow.Services.Auth.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;
    private readonly IMapper _mapper;
    public AuthService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ITokenService tokenService,
        IOptions<JwtSettings> jwtOptions,
        IMapper mapper,
        ILogger<AuthService> logger)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _jwtSettings = jwtOptions.Value;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ServiceResult<AuthResponseDTO>> LoginAsync(
        LoginDTO dto,
        CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Identifier) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return ServiceResult<AuthResponseDTO>.Fail(
                    ResultStatus.ValidationError,
                    "Username/email and password are required.");
            }

            var user = await _userRepository.GetUserByEmailOrUserNameAsync(
                dto.Identifier,
                ct);

            if (user is null)
            {
                return ServiceResult<AuthResponseDTO>.Fail(
                    ResultStatus.Unauthorized,
                    "Invalid username/email or password.");
            }

            var isPasswordValid = HashingHelper.VerifyPasswordHash(
                dto.Password,
                user.PasswordHash,
                user.PasswordSalt);

            if (!isPasswordValid)
            {
                return ServiceResult<AuthResponseDTO>.Fail(
                    ResultStatus.Unauthorized,
                    "Invalid username/email or password.");
            }

            var expiresAt = DateTime.UtcNow.AddMinutes(
                _jwtSettings.AccessTokenExpirationMinutes);

            var accessToken = _tokenService.CreateAccessToken(user, expiresAt);

            var response = new AuthResponseDTO
            {
                AccessToken = accessToken,
                AccessTokenExpiration = expiresAt
            };

            return ServiceResult<AuthResponseDTO>.Ok(
                response,
                "Login successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while logging in user.");

            return ServiceResult<AuthResponseDTO>.Fail(
                ResultStatus.Error,
                "An error occurred while logging in.");
        }
    }

    public async Task<ServiceResult<AuthResponseDTO>> RegisterAsync(
    RegisterDTO dto,
    CancellationToken ct = default)
    {
        try
        {
            if (dto is null)
            {
                return ServiceResult<AuthResponseDTO>.Fail(
                    ResultStatus.ValidationError,
                    "Register request cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(dto.UserName) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return ServiceResult<AuthResponseDTO>.Fail(
                    ResultStatus.ValidationError,
                    "Username, email and password are required.");
            }

            var userName = dto.UserName.Trim();
            var email = dto.Email.Trim();

            var userExistsWithEmail = await _userRepository.UserExistsByEmailAsync(
                email,
                ct);

            if (userExistsWithEmail)
            {
                return ServiceResult<AuthResponseDTO>.Fail(
                    ResultStatus.Conflict,
                    $"There is already a user with {email} email.");
            }

            var userExistsWithUserName = await _userRepository.UserExistsByUserNameAsync(
                userName,
                ct);

            if (userExistsWithUserName)
            {
                return ServiceResult<AuthResponseDTO>.Fail(
                    ResultStatus.Conflict,
                    $"There is already a user with {userName} username.");
            }

            var defaultRole = await _roleRepository.GetRoleByNameAsync(
                AuthRoleConstants.DefaultRegisterRole,
                ct);

            if (defaultRole is null)
            {
                _logger.LogError(
                    "Default register role was not found. RoleName: {RoleName}",
                    AuthRoleConstants.DefaultRegisterRole);

                return ServiceResult<AuthResponseDTO>.Fail(
                    ResultStatus.Error,
                    "Default user role is not configured.");
            }

            var user = _mapper.Map<User>(dto);

            user.Roles.Add(defaultRole);

            await _userRepository.AddUserAsync(user, ct);
            await _userRepository.SaveChangesAsync(ct);

            var expiresAt = DateTime.UtcNow.AddMinutes(
                _jwtSettings.AccessTokenExpirationMinutes);

            var accessToken = _tokenService.CreateAccessToken(user, expiresAt);

            var response = new AuthResponseDTO
            {
                AccessToken = accessToken,
                AccessTokenExpiration = expiresAt
            };

            return ServiceResult<AuthResponseDTO>.Ok(
                response,
                "Register successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while registering user.");

            return ServiceResult<AuthResponseDTO>.Fail(
                ResultStatus.Error,
                "An error occurred while registering user.");
        }
    }
    public async Task<ServiceResult<MeDTO>> GetMeAsync(
    int userId,
    CancellationToken ct = default)
    {
        try
        {
            var user = await _userRepository.GetUserByIdWithRolesAsync(userId, ct);

            if (user is null)
            {
                return ServiceResult<MeDTO>.Fail(
                    ResultStatus.NotFound,
                    "User not found.");
            }

            var response = new MeDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = user.Roles
                    .Select(r => r.Name)
                    .ToList()
            };

            return ServiceResult<MeDTO>.Ok(
                response,
                "User information found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting current user. UserId: {UserId}", userId);

            return ServiceResult<MeDTO>.Fail(
                ResultStatus.Error,
                "An error occurred while getting current user.");
        }
    }
}