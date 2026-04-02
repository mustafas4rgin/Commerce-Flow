using AutoMapper;
using CommerceFlow.Services.Auth.Application.DTOs.Role;
using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Application.Profiles;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role,CreateRoleDTO>().ReverseMap();
        CreateMap<Role,UpdateRoleDTO>().ReverseMap();
    }
}