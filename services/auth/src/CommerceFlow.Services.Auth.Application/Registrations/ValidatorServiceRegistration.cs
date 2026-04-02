using CommerceFlow.Services.Auth.Application.Providers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CommerceFlow.Services.Auth.Application.Registrations;

public static class ValidatorServiceRegistration
{
     public static IServiceCollection AddEntityValidators(this IServiceCollection services)
    {
        var entityValidatorAssemblies = EntityValidatorAssemblyProvider.GetValidatorAssemblies();

        foreach (var assemblyType in entityValidatorAssemblies)
            services.AddValidatorsFromAssemblyContaining(assemblyType);

        return services;
    }
}