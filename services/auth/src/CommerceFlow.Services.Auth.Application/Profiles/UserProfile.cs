using AutoMapper;
using CommerceFlow.Services.Auth.Application.DTOs.User;
using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDTO, User>()
        .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
        .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
        .ForMember(dest => dest.Roles, opt => opt.Ignore())
        .AfterMap((src, dest) =>
        {
            if (!string.IsNullOrWhiteSpace(src.Password))
            {
                HashingHelper.CreatePasswordHash(src.Password, out var hash, out var salt);
                dest.PasswordHash = hash;
                dest.PasswordSalt = salt;
            }
        });
        
        CreateMap<UpdateUserDTO, User>()
        .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
        .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
        .ForMember(dest => dest.Roles, opt => opt.Ignore())
        .AfterMap((src, dest) =>
        {
            var passwordProp = typeof(UpdateUserDTO).GetProperty("Password");
            var newPassword = passwordProp?.GetValue(src) as string;
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                HashingHelper.CreatePasswordHash(newPassword, out var hash, out var salt);
                dest.PasswordHash = hash;
                dest.PasswordSalt = salt;
            }
        });

        CreateMap<User, UserDTO>().ReverseMap();
    }
}