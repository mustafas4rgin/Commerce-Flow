using CommerceFlow.Services.Auth.Application.DTOs.Auth;
using CommerceFlow.Shared.Results;

namespace CommerceFlow.Services.Auth.Application.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<AuthResponseDTO>> LoginAsync(
        LoginDTO dto,
        CancellationToken ct = default);
}