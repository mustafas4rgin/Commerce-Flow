using AutoMapper;
using CommerceFlow.Services.Auth.Application.DTOs.Auth;
using CommerceFlow.Services.Auth.Application.DTOs.User;
using CommerceFlow.Services.Auth.Domain.Entities;

namespace CommerceFlow.Services.Auth.Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDTO, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.Trim()))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                if (!string.IsNullOrWhiteSpace(src.Password))
                {
                    HashingHelper.CreatePasswordHash(
                        src.Password,
                        out var hash,
                        out var salt);

                    dest.PasswordHash = hash;
                    dest.PasswordSalt = salt;
                }
            });

        CreateMap<RegisterDTO, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.Trim()))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                if (!string.IsNullOrWhiteSpace(src.Password))
                {
                    HashingHelper.CreatePasswordHash(
                        src.Password,
                        out var hash,
                        out var salt);

                    dest.PasswordHash = hash;
                    dest.PasswordSalt = salt;
                }
            });

        CreateMap<UpdateUserDTO, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.Trim()))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore());

        CreateMap<User, UserDTO>();
    }
}