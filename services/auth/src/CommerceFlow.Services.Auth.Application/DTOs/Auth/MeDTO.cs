namespace CommerceFlow.Services.Auth.Application.DTOs.Auth;

public sealed class MeDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
}