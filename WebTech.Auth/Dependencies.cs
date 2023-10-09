using WebTech.Auth.Data.DataSeeder;
using WebTech.Auth.Services.Implementations.Auth;
using WebTech.Auth.Services.Implementations.User;
using WebTech.Auth.Services.Interfaces.Auth;
using WebTech.Auth.Services.Interfaces.User;

namespace WebTech.Auth;

public static class Dependencies
{
    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthAppService, AuthAppService>();
        services.AddScoped<IDataSeeder, DataSeeder>();
        services.AddScoped<IUserService, UserService>();
    }
}