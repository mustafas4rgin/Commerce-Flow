using CommerceFlow.Services.Auth.Application.DTOs.Auth;
using CommerceFlow.Services.Auth.Application.Interfaces;
using CommerceFlow.Services.Auth.Application.Options;
using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFlow.Shared.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommerceFlow.Services.Auth.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IOptions<JwtSettings> jwtOptions,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _jwtSettings = jwtOptions.Value;
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
}