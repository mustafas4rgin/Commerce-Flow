using CommerceFlow.Shared.BuildingBlocks;

namespace CommerceFlow.Services.Auth.Domain.Entities;

public class User : EntityBase
{
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    //nav prop
    public ICollection<Role> Roles { get; set; } = null!;
}