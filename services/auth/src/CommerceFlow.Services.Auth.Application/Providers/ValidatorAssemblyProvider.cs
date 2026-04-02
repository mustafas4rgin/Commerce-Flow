using CommerceFlow.Services.Auth.Application.Validators;

namespace CommerceFlow.Services.Auth.Application.Providers;

public static class EntityValidatorAssemblyProvider
{
    public static Type[] GetValidatorAssemblies()
    {
        return new[]
        {
            typeof(RoleValidator),
            typeof(UserValidator),
        };
    }
}