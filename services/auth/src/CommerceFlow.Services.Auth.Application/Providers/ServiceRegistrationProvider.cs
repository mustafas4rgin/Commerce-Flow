using CommerceFlow.Services.Auth.Application.Interfaces;
using CommerceFlow.Services.Auth.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CommerceFlow.Services.Auth.Application.Providers;

public class ServiceRegistrationProvider
{
    public static void RegisterServices(IServiceCollection services)
    {
        var servicesToRegister = new (Type Interface, Type Implementation)[]
        {
            (typeof(IRoleService),typeof(RoleService)),
        };

        foreach (var service in servicesToRegister)
        {
            services.AddTransient(service.Interface, service.Implementation);
        }
    }
}