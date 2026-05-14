
using CommerceFlow.Services.Auth.Application.Interfaces;
using CommerceFlow.Services.Auth.Infrastructure.Contexts;
using CommerceFlow.Services.Auth.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommerceFlow.Services.Auth.Infrastructure;

public static class DataServiceRegistration
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("AuthDatabase"));
        });

        RepositoryRegistrationProvider.RegisterRepositories(services);

        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}