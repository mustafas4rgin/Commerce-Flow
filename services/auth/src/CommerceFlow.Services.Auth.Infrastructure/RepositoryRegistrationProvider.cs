using CommerceFlow.Services.Auth.Domain.Contracts;
using CommerceFLow.Services.Auth.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CommerceFlow.Services.Auth.Infrastructure;


public class RepositoryRegistrationProvider
{
    public static void RegisterRepositories(IServiceCollection services)
    {
        var servicesToRegister = new (Type Interface, Type Implementation)[]
        {
            (typeof(IRoleRepository),typeof(RoleRepository)),
            (typeof(IAuthRepository), typeof(AuthRepository)),
        };
        foreach (var service in servicesToRegister)
        {
            services.AddTransient(service.Interface, service.Implementation);
        }
    }
}