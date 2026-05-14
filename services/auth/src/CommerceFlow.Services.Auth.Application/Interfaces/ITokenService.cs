using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Application.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(User user, DateTime expiresAt);
}