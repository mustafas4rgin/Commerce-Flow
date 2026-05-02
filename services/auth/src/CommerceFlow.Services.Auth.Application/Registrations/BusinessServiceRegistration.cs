using CommerceFlow.Services.Auth.Application.Profiles;
using CommerceFlow.Services.Auth.Application.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace CommerceFlow.Services.Auth.Application.Registrations;

public static class BusinessServiceRegistration
{
    public static IServiceCollection AddBusinessService(this IServiceCollection services)
    {
        ServiceRegistrationProvider.RegisterServices(services);

        services.AddAutoMapper(_ => { }, typeof(RoleProfile).Assembly);
        services.AddEntityValidators();

        return services;
    }
}