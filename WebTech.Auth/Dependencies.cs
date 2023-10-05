using WebTech.Auth.Services.Implementations.Auth;
using WebTech.Auth.Services.Interfaces.Auth;

namespace WebTech.Auth;

public static class Dependencies
{
    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthAppService, AuthAppService>();
    }
}