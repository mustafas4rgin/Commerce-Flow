using CommerceFlow.Shared.BuildingBlocks;

namespace CommerceFlow.Services.Auth.Domain.Entities;

public class Role : EntityBase
{
    public string Name { get; set; } = string.Empty;
}